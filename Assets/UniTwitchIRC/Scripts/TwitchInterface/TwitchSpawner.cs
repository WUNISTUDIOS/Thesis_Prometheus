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

using IRCnect.Channel.Monitor;
using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using System.Collections;
using System.Collections.Generic;
using UniTwitchIRC.Controllers;
using UniTwitchIRC.TwitchInterface.MonitorEvents;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Spawn an item for a viewer from a chat message command
    /// </summary>
    public class TwitchSpawner : MonoBehaviour
    {
        /// <summary>
        /// Access to the Twitch chat client
        /// </summary>
        [Header("Twitch Controllers")]
        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        protected TwitchChat m_TwitchChat = null;

        /// <summary>
        /// Access to the players queue for spawn and validation
        /// </summary>
        [SerializeField, RequiredInHierarchy(typeof(TwitchPlayerQueue))]
        protected TwitchPlayerQueue m_TwitchPlayerQueue;

        /// <summary>
        /// Access to the monitor number event handler
        /// </summary>
        [SerializeField, RequiredInHierarchy(typeof(MonitorNumberEvents))]
        protected MonitorNumberEvents m_MonitorNumberEvents;

        /// <summary>
        /// Access to the monitor string event handler
        /// </summary>
        [SerializeField, RequiredInHierarchy(typeof(MonitorStringEvents))]
        protected MonitorStringEvents m_MonitorStringsEvents;

        [Header("Spawn Information")]
        [TwitchCommand]
        [SerializeField]
        string m_SpawnCommand = "spawn";

        [SerializeField]
        Vector3 m_Offset = Vector3.up * 2.0f;

        [SerializeField]
        float m_Rate = 1.0f;

        [Header("Prefabs")]
        [SerializeField]
        TwitchPlayerController m_PlayerController = null;

        [SerializeField, Tooltip("Any other prefabs to spawn along with the player controller.")]
        GameObject[] m_Others = null;

        /// <summary>
        /// The current queued player not spawned yet
        /// </summary>
        protected Queue<string> m_Queue = null;
        
        /// <summary>
        /// The players spawned
        /// </summary>
        protected Dictionary<string, List<GameObject>> m_Objects = null;

        /// <summary>
        /// Delegate to identify a spawn request sent from the player queue
        /// </summary>
        /// <param name="nick">The nick/viewer requesting the spawn</param>
        public delegate void SpawnRequested(string nick);

        /// <summary>
        /// Invoked when the spawn command is received and the player is validated
        /// </summary>
        public event SpawnRequested onSpawnRequested;

        Transform m_Trans;

        void Awake()
        {
            m_Trans = transform;
            m_Queue = new Queue<string>();
            m_Objects = new Dictionary<string, List<GameObject>>();
        }

        void OnEnable()
        {
            CommandsBehaviour.OnCommandsReceived -= CommandsBehaviour_OnCommandsReceived;
            CommandsBehaviour.OnCommandsReceived += CommandsBehaviour_OnCommandsReceived;
        }

        void OnDisable()
        {
            CommandsBehaviour.OnCommandsReceived -= CommandsBehaviour_OnCommandsReceived;
        }

        void CommandsBehaviour_OnCommandsReceived(TwitchChat twitchChat, CommandsArgs args)
        {
            Spawn(args);
        }

        IEnumerator Start()
        {
            m_TwitchPlayerQueue.onValidated += m_TwitchPlayerQueue_onValidated;
            while(enabled)
            {
                if(m_Queue.Count > 0)
                {
                    string nick = m_Queue.Dequeue();
                    TwitchPlayerController playerController = SpawnPlayerPrefabs(nick);
                    m_MonitorStringsEvents.onStringArray.AddListener(playerController.OnDisplayNameChange);

                    m_MonitorNumberEvents.onTwitchCommand.AddListener(playerController.OnReset);

                    m_MonitorNumberEvents.onTwitchCommand.AddListener(playerController.OnMoveUniDirection);
                    m_MonitorNumberEvents.onTwitchCommand.AddListener(playerController.OnMove);
                }
                yield return new WaitForSeconds(m_Rate);
            }
        }

        void m_TwitchPlayerQueue_onValidated(TwitchPlayerQueue.Player player)
        {
            string nick = player.nick;
            AddNickToObjectsList(nick);
        }

        /// <summary>
        /// Spawn a user object for a player
        /// <para>Override in derived classes to Instantiate additional objects and associate them with the PlayerController</para>
        /// </summary>
        /// <param name="nick">The viewer requesting the spawn</param>
        protected virtual TwitchPlayerController SpawnPlayerPrefabs(string nick)
        {

            TwitchPlayerController playerController = GameObject.Instantiate<TwitchPlayerController>(m_PlayerController);
            playerController.StartNew(nick, m_Trans.position + m_Offset);
            playerController.gameObject.name = string.Concat(m_PlayerController.name, ".", nick);

            if(!m_Objects[nick].Contains(playerController.gameObject))
            {
                m_Objects[nick].Add(playerController.gameObject);
                PlayerPrefabInstantiated(playerController, playerController.gameObject);
            }

            for(int i = 0; i < m_Others.Length; i++)
            {
                GameObject otherGO = GameObject.Instantiate<GameObject>(m_Others[i]);
                if(!m_Objects[nick].Contains(otherGO))
                {
                    m_Objects[nick].Add(otherGO);
                    PlayerPrefabInstantiated(playerController, otherGO);
                }
            }
            return playerController;
        }

        /// <summary>
        /// Gives derived classes an oppotunity to modify the GameObject
        /// </summary>
        /// <param name="playerController">Player controller created</param>
        /// <param name="playerGameObject">Player associated GameObject created</param>
        protected virtual void PlayerPrefabInstantiated(TwitchPlayerController playerController, GameObject playerGameObject) { }

        /// <summary>
        /// Spawn a user object for a player
        /// </summary>
        /// <param name="monitorArgs">Raw Command args to check for spawn command</param>
        public void Spawn(MonitorArgs monitorArgs)
        {
            CommandsArgs commandsArgs = monitorArgs as CommandsArgs;
            if(commandsArgs.IsCommand(m_SpawnCommand))
            {
                Spawn(commandsArgs.nick);
            }
        }

        /// <summary>
        /// Spawn a user object for a player
        /// </summary>
        /// <param name="nick">The viewer requesting the spawn</param>
        public void Spawn(string nick)
        {
            if(nick == m_TwitchChat.messenger.channel)
            {
                AddNickToObjectsList(nick);
            }
            if(m_Objects.ContainsKey(nick))
            {
                m_Queue.Enqueue(nick);
            }
            if(onSpawnRequested != null)
            {
                onSpawnRequested.Invoke(nick);
            }
        }

        void AddNickToObjectsList(string nick)
        {
            if(!m_Objects.ContainsKey(nick))
            {
                m_Objects.Add(nick, new List<GameObject>());
            }
        }
    }
}
