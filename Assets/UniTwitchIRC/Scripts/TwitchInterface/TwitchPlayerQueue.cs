#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: UniTwirchIRC
     
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using TwitchConnectTv;
using UniTwitchIRC.TwitchInterface.MonitorEvents;
using UnityEngine;
using UnityEngine.Networking;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Maintains a queue of the players
    /// <para>Also checks the follower staus of the viewer</para>
    /// </summary>
    public class TwitchPlayerQueue : MonoBehaviour
    {
        const string JSON_EXECPTION_MESSAGE = "Exception: {0}";

        /// <summary>
        /// Reference to the player object
        /// </summary>
        [System.Serializable]
        public class Player
        {
            /// <summary>
            /// Name shown in the scene
            /// </summary>
            public string displayName;

            /// <summary>
            /// Index in player queue
            /// </summary>
            public int index;

            /// <summary>
            /// The Twitch chat user name
            /// </summary>
            public string nick;

            /// <summary>
            /// Pool for viewer/player information has been sent and received.
            /// </summary>
            public bool validated;

            /// <summary>
            /// Player follows channel
            /// </summary>
            public bool follows;

            /// <summary>
            /// Constructor
            /// <para>Creates a new player.</para>
            /// </summary>
            /// <param name="index">Index in the player queue array</param>
            /// <param name="nick">The Twitch chat user name</param>
            public Player(int index, string nick)
            {
                this.index = index;
                this.nick = nick;
                this.displayName = this.nick;
            }
        }
        [SerializeField, RequiredInHierarchy(typeof(TwitchConnect))]
        TwitchConnect m_TwitchConnect = null;

        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        TwitchChat m_TwitchChat = null;

        [SerializeField, RequiredInHierarchy(typeof(MonitorCommandsEvents))]
        MonitorCommandsEvents m_MonitorCommands = null;

        [SerializeField]
        float m_RefreshDelay = 15;

        [Header("Commands")]
        [SerializeField, TwitchCommand]
        string m_JoinCommand = "join";
        [SerializeField, Tooltip("The command nick must be a follower of the channel to join.")]
        bool m_JoinMustFollow = false;
        [SerializeField, Tooltip("{0} will be replaced with the nick.")]
        string m_JoinFirstFormat = "Opps, {0} you must !join first.";

        [Space]
        [SerializeField, TwitchCommand]
        string m_NameCommand = "name";
        [SerializeField, Tooltip("The command nick must be a follower of the channel to change the display name.")]
        bool m_NameMustFollow = false;
        [SerializeField, Tooltip("{0} will be replaced with the nick.")]
        string m_FollowFirstFormat = "Hey, {0}, you must be a follower to change your display name.  Follow use the join command again then try to change your display name.";


        [Header("Command Responces")]
        [Header("Followers")]
        [SerializeField]
        string m_followerResponce = "Thanks for being a Follower!";
        [SerializeField]
        string m_NotFollowerResponce = "Awww y u no Follow?";

        [Header("Join")]
        [SerializeField, Tooltip("{0} will be replaced with the nick.  The {1} with the Follower Responce or Not Follower Responce")]
        string m_JoinResponceFormat = "{0} has joined. {1}";


        [Header("Others")]
        [SerializeField, Tooltip("{0} will be replaced with the nick..  The {1} with the index quued at.")]
        string m_AlreadyQueuedFormat = "{0} already queued at {1}";

        [SerializeField, Tooltip("DO NOT EDIT: For your runtime information only to see how the values queued players data are handled.")]
        List<Player> m_PlayerQueue = null;

        /// <summary>
        /// Delegate to identify a player validation status
        /// <para>A request has been made to twitch API for follows status</para>
        /// </summary>
        /// <param name="player">The player that has been validated</param>
        public delegate void Validated(Player player);

        /// <summary>
        /// Delegate to identify a join and name change request sent
        /// </summary>
        /// <param name="nick">The nick/viewer requesting the join or name change commands</param>
        public delegate void Requested(string nick);

        /// <summary>
        /// Invoked when the join command is received and the player is validated
        /// </summary>
        public event Requested onJoinRequested;

        /// <summary>
        /// Invoked when the name command is received and the player is validated
        /// </summary>
        public event Requested onNameChangeRequested;

        /// <summary>
        /// Invoked when the request has been returened for validation
        /// </summary>
        public event Validated onValidated;

        void Start()
        {
            m_MonitorCommands.onCommand.AddListener(OnJoinRecieved);
            m_MonitorCommands.onArgument.AddListener(OnNameRecieved);
        }

        /// <summary>
        /// Invoked when a join command is received
        /// <para>The command must be validated within the method</para>
        /// </summary>
        /// <param name="nick">The nick/viewer who sent the command</param>
        /// <param name="command">The command received</param>
        public void OnJoinRecieved(string nick, string command)
        {
            if(command.IsCommand(m_JoinCommand))
            {
                if(onJoinRequested != null)
                {
                    onJoinRequested.Invoke(nick);
                }
                StartCoroutine(JoinQueue(nick));
            }
        }

        /// <summary>
        /// Invoked when a name change command is received
        /// <para>The command must be validated within the method</para>
        /// </summary>
        /// <param name="nick">The nick/viewer who sent the command</param>
        /// <param name="command">The command received</param>
        /// <param name="argument">The argument received containing the new name to display</param>
        public void OnNameRecieved(string nick, string command, string argument)
        {
            if(command.IsCommand(m_NameCommand))
            {
                if(onNameChangeRequested != null)
                {
                    onNameChangeRequested.Invoke(nick);
                }
                int index = m_PlayerQueue.FindIndex(x => x.nick == nick);
                if(index > -1)
                {
                    if(m_NameMustFollow)
                    {
                        if(m_PlayerQueue[index].follows)
                        {
                            m_PlayerQueue[index].displayName = argument;
                        }
                        else
                        {
                            m_TwitchChat.SendChatMessageFormat(m_FollowFirstFormat, nick);
                        }
                    }
                    else
                    {
                        m_PlayerQueue[index].displayName = argument;
                    }
                }
                else
                {
                    m_TwitchChat.SendChatMessageFormat(m_JoinFirstFormat, nick);
                }
            }
        }

        IEnumerator JoinQueue(string nick)
        {
            yield return null;
            int index = m_PlayerQueue.FindIndex(x => x.nick == nick);
            if(index > -1)
            {
                if(!m_PlayerQueue[index].follows)
                {
                    yield return StartCoroutine(CheckFollower(m_PlayerQueue[index]));
                    yield break;
                }
                m_TwitchChat.SendChatMessageFormat(m_AlreadyQueuedFormat, nick, index);
                yield break;
            }
            Player player = new Player(m_PlayerQueue.Count, nick);
            m_PlayerQueue.Add(player);

            yield return StartCoroutine(CheckFollower(player));

        }

        IEnumerator CheckFollower(Player player)
        {
            player.validated = false;

            float refreshDelay = m_RefreshDelay;
            string url = m_TwitchConnect.broadcaster.url.GetIsFollowerUrl(player.nick);
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return www.SendWebRequest();
                
                if (www.isNetworkError)
                {
                    string[] pages = url.Split('?');
                    Debug.LogError(pages[0] + ": Error: " + www.error);
                }
                else
                {
                    //Debug.Log(pages[page] + ":\nReceived: " + www.downloadHandler.text);

                    string rawResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    string result = rawResult.Replace("\n", "").Replace("\r", "");

                    TwitchTv.API.IsFollower twitchTMI = null;

                    try
                    {
                        twitchTMI = m_TwitchConnect.broadcaster.ParseJsonIsFollower(result);
                        player.validated = true;
                        player.follows = twitchTMI.Confirmed();

                        if (m_JoinMustFollow && (!player.follows && player.nick != m_TwitchChat.messenger.channel))
                        {
                            m_PlayerQueue.Remove(player);
                        }
                        if (player.nick != m_TwitchChat.messenger.channel)
                        {
                            string followMessage = player.follows ? m_followerResponce : m_NotFollowerResponce;
                            m_TwitchChat.SendChatMessageFormat(m_JoinResponceFormat, player.nick, followMessage);
                        }
                        else
                        {
                            player.follows = true;
                        }
                        if (m_PlayerQueue.Contains(player))
                        {
                            if (onValidated != null)
                            {
                                onValidated.Invoke(player);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogFormat(JSON_EXECPTION_MESSAGE, $"{ex.Message}");
                        refreshDelay = 0.0f;
                    }
                }
            }

            yield return null;

        }

    }
}