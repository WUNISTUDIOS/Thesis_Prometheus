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

using PasswordProtector;
using System;
using System.Collections;
using TwitchConnectTv;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Provides a connection to the Twitch API and TMI information
    /// <para>This class polls for the viewers in the chat room at the present time</para>
    /// </summary>
    public class TwitchConnect : MonoBehaviour
    {
        const string JSON_EXECPTION_MESSAGE = "<color=red>Exception</color>: {0}\n<color=green>Trying to read jsonn again from stream in {1} seconds.</color>";

        /// <summary>
        /// Event invoked when the JSON data has been refreshed or reloaded
        /// </summary>
        [System.Obsolete("Use OnRefreshedPolling for event subscription", false)]
        public static event Action<TwitchChat, TwitchTv.TMI> OnRefreshed;
        public static event Action<EventArgs> OnRefreshedPolling;

        public class RefreshArgs<T> : EventArgs
        {
            public readonly TwitchChat twitchChat;
            public readonly T result;
            public RefreshArgs(TwitchChat twitchChat, T result)
            {
                this.twitchChat = twitchChat;
                this.result = result;
            }
        }

        /*
        /// <summary>
        /// Event invoked when the JSON data has been refreshed or reloaded
        /// </summary>
        public static event Action<TwitchChat, TwitchTv.API.Followers> OnRefreshedFollows;
        */
        [PasswordProtect]
        [SerializeField, Tooltip("This is the client id for connection to Twitch API.")]
        string m_ClientId = string.Empty;

        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        TwitchChat m_TwitchIRC = null;

        [SerializeField, Tooltip("Delay between the polling for chat room information.")]
        float m_RefreshDelay = 60.0f;

        [SerializeField, Tooltip("Show the JSON exception formatted in the debug Console window.\nSeeing this execption usually means a misses responce and will be resolved when the connection is succusful.")]
        bool m_DebugLog = true;

        [SerializeField, Tooltip("Collection of present chat viewers.")]
        TwitchTv.TMI.Chatters m_Chatters = null;

        [SerializeField, Tooltip("Collection of recent stream followers.")]
        TwitchTv.API.Followers.Follows[] m_Follows = null;
        
        /// <summary>
        /// Collection of present chat viewers.
        /// </summary>
        public TwitchTv.TMI.Chatters chatters { get { return m_Chatters; } }

        public Broadcaster broadcaster { get; private set; } = null;

        void Start()
        {
            broadcaster = new Broadcaster(m_TwitchIRC.messenger.channel, m_ClientId);

            StartCoroutine(EndpointPolling<TwitchTv.TMI>(broadcaster.url.chatters, m_RefreshDelay, (result) =>
            {
                m_Chatters = result.chatters;
                OnRefreshedPolling?.Invoke(new RefreshArgs<TwitchTv.TMI>(m_TwitchIRC, result));
                OnRefreshed?.Invoke(m_TwitchIRC, result);
            }, m_DebugLog));

            StartCoroutine(EndpointPolling<TwitchTv.API.Followers>($"{broadcaster.url.follows}&limit={5}", m_RefreshDelay, (result) =>
            {
                m_Follows = result.follows;
                OnRefreshedPolling?.Invoke(new RefreshArgs<TwitchTv.API.Followers>(m_TwitchIRC, result));
            }, m_DebugLog));
        }
        
        static IEnumerator EndpointPolling<T>(string url, float refreshDelay, Action<T> callback, bool debugLog)
        {
            while(true)
            {
                using (UnityWebRequest www = UnityWebRequest.Get(url))
                {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError)
                    {
                        string[] pages = url.Split('?');
                        Debug.LogError(pages[0] + ": Error: " + www.error);
                    }
                    else
                    {
                        try
                        {
                            string rawResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                            string json = rawResult.Replace("\n", "").Replace("\r", "");
                            T result = TwitchURI.ParseJson<T>(json);
                            callback.Invoke(result);
                        }
                        catch (Exception ex)
                        {
                            if(debugLog)
                            {                            
                                Debug.LogFormat(JSON_EXECPTION_MESSAGE, ex.Message, refreshDelay);
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(refreshDelay);
            }
        }
    }
}
