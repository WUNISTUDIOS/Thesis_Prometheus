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

using IRCnect.Channel.Monitor.Replies.Inbounds;
using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TwitchConnectTv;
using UniTwitchIRC.Controllers;
using UniTwitchIRC.TwitchInterface;
using UnityEngine;
using UnityEngine.Networking;

namespace UniTwitchIRC.ExampleUI
{
    /// <summary>
    /// Status UI is the lobby UI to show the player/viewer in the lobby
    /// </summary>
    public class StatusUI : MonoBehaviour
    {
        /// <summary>
        /// GameObject references for the rect transform and prefab for the status ui
        /// </summary>
        [System.Serializable]
        public class StatusItem
        {
            /// <summary>
            /// The data structure to hold the prefab and rect transform references
            /// </summary>
            [System.Serializable]
            public class Item
            {
                /// <summary>
                /// The prefab reference for this status ui
                /// </summary>
                public RectTransform prefab;
                /// <summary>
                /// The rect transform reference for this status ui
                /// </summary>
                public RectTransform panel;
            }
            /// <summary>
            /// Item for the accolades wether the viewer is a follower and or playing
            /// </summary>
            public Item accolades;
            /// <summary>
            /// Item for the nick for the viewer
            /// </summary>
            public Item chatters;
        }

        class Viewer
        {
            public readonly string channel;
            public readonly AccoladesUI accoladesUI;
            public readonly ChattersUI chattersUI;

            public float lifeTime;
            public bool isFollower;
            public bool isPlaying;

            public Viewer(string nick, StatusItem statusItem, string channel)
            {
                this.channel = channel;

                RectTransform chattersRectTrans = GameObject.Instantiate<RectTransform>(statusItem.chatters.prefab);
                chattersRectTrans.SetParent(statusItem.chatters.panel);
                chattersRectTrans.localScale = Vector3.one;
                this.chattersUI = chattersRectTrans.GetComponent<ChattersUI>();
                this.chattersUI.gameObject.SetActive(true);
                this.chattersUI.pointsText.text = string.Format("{0:00}", 0);
                this.chattersUI.chatterText.text = nick;
                this.chattersUI.timerText.text = string.Format("{0:000}", 0);

                RectTransform accoladesRectTrans = GameObject.Instantiate<RectTransform>(statusItem.accolades.prefab);
                accoladesRectTrans.SetParent(statusItem.accolades.panel);
                accoladesRectTrans.localScale = Vector3.one;
                this.accoladesUI = accoladesRectTrans.GetComponent<AccoladesUI>();
                this.accoladesUI.gameObject.SetActive(true);
                this.accoladesUI.followerImage.color = Color.yellow;
                this.accoladesUI.playingImage.color = Color.yellow;
            }
        }

        [SerializeField, RequiredInHierarchy(typeof(TwitchConnect))]
        TwitchConnect m_TwitchConnect = null;

        [SerializeField, RequiredInHierarchy(typeof(CommandsBehaviour))]
        CommandsBehaviour m_CommandsBehaviour = null;

        [SerializeField, TwitchCommand]
        string m_SpawnCommand = "spawn";

        [SerializeField]
        float m_LifeTime = 240.0f;

        [SerializeField]
        StatusItem m_StatusItem = null;

        Dictionary<string, Viewer> m_Viewers;

        void Awake()
        {
            m_Viewers = new Dictionary<string, Viewer>();
        }

        IEnumerator Start()
        {
            m_CommandsBehaviour.onReceived += m_CommandsBehaviour_onReceived;
            while(enabled)
            {
                var viewers = new Dictionary<string, Viewer>(m_Viewers);
                foreach(var viewer in viewers)
                {
                    viewer.Value.lifeTime += Time.deltaTime;
                    float timeLeft = m_LifeTime - viewer.Value.lifeTime;
                    viewer.Value.chattersUI.timerText.text = string.Format("{0:000}", timeLeft);
                    if(viewer.Value.lifeTime >= m_LifeTime)
                    {
                        Destroy(viewer.Value.accoladesUI.gameObject);
                        Destroy(viewer.Value.chattersUI.gameObject);
                        m_Viewers.Remove(viewer.Key);
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }

        void m_CommandsBehaviour_onReceived(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            if(commandsArgs.IsCommand(m_SpawnCommand))
            {
                if(m_Viewers.ContainsKey(commandsArgs.nick))
                {
                    Viewer viewer = m_Viewers[commandsArgs.nick];
                    viewer.accoladesUI.playingImage.color = Color.green;
                    viewer.isPlaying = true;
                }
            }
        }

        void OnEnable()
        {
            GreetingsBehaviour.OnGreetingsReceived -= InboundsBehaviour_OnGreeted;
            GreetingsBehaviour.OnGreetingsReceived += InboundsBehaviour_OnGreeted;

            TwitchConnect.OnRefreshedPolling -= TwitchConnect_OnRefreshedPolling;
            TwitchConnect.OnRefreshedPolling += TwitchConnect_OnRefreshedPolling;
            
            PlayerPickupController.OnPickUp -= PlayerPickupController_OnPickUp;
            PlayerPickupController.OnPickUp += PlayerPickupController_OnPickUp;            
        }

        void OnDisable()
        {
            GreetingsBehaviour.OnGreetingsReceived -= InboundsBehaviour_OnGreeted;

            TwitchConnect.OnRefreshedPolling -= TwitchConnect_OnRefreshedPolling;

            PlayerPickupController.OnPickUp -= PlayerPickupController_OnPickUp;
        }

        void PlayerPickupController_OnPickUp(TwitchPlayerController playerController, GameObject otherGameObject)
        {
            if(m_Viewers.ContainsKey(playerController.nick))
            {
                m_Viewers[playerController.nick].chattersUI.pointsText.text = string.Format("{0:00}", playerController.points);
            }
        }

        void TwitchConnect_OnRefreshedPolling(EventArgs e)
        {
            TwitchConnect.RefreshArgs<TwitchTv.TMI> obj = e as TwitchConnect.RefreshArgs<TwitchTv.TMI>;
            TwitchTv.TMI twitchTvTMI = obj.result;
            if (twitchTvTMI == null)
            {
                Debug.LogFormat("twitchTvTMI is still null {0}", DateTime.Now);
            }
            //string moderators = string.Join(", ", twitchTvTMI.chatters.moderators);
            //string viewers = string.Join(", ", twitchTvTMI.chatters.viewers);
            //Debug.LogFormat("moderators:{0} viewers:{1} > {2}", moderators, viewers, DateTime.Now);

            foreach (var item in twitchTvTMI.chatters.viewers.Concat(twitchTvTMI.chatters.moderators))
            {
                if (m_Viewers.ContainsKey(item))
                {
                    m_Viewers[item].lifeTime = 0.0f;
                }
            }
            StartCoroutine(CheckForFollowers(twitchTvTMI.chatters.viewers.Concat(twitchTvTMI.chatters.moderators).ToArray()));
        }

        void InboundsBehaviour_OnGreeted(TwitchChat twitchIRC, InboundsArgs obj)
        {
            if(!m_Viewers.ContainsKey(obj.nick))
            {
                Viewer viewer = new Viewer(obj.nick, m_StatusItem, twitchIRC.messenger.channel);
                viewer.lifeTime = 0.0f;
                m_Viewers.Add(obj.nick, viewer);
            }
            else
            {
                m_Viewers[obj.nick].lifeTime = 0.0f;
            }
            StartCoroutine(CheckForFollowers(new[] { obj.nick }));
        }
        
        IEnumerator CheckForFollowers(string[] chatters)
        {
            foreach(var nick in chatters)
            {

                Viewer viewer;
                if(m_Viewers.TryGetValue(nick, out viewer))
                {
                    if(nick == m_TwitchConnect.broadcaster.channel)
                    {
                        viewer.accoladesUI.followerImage.color = Color.green;
                        continue;
                    }
                    if(!viewer.isFollower)
                    {

                        string url = m_TwitchConnect.broadcaster.url.GetIsFollowerUrl(nick);
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

                                viewer.isFollower = m_TwitchConnect.broadcaster.ParseJsonIsFollower(result).Confirmed();
                                if (viewer.isFollower)
                                {
                                    viewer.accoladesUI.followerImage.color = Color.green;
                                }
                                else
                                {
                                    viewer.accoladesUI.followerImage.color = Color.red;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
