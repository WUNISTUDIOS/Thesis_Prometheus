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

using UnityEngine;

namespace UniTwitchIRC.TwitchInterface.MonitorEvents
{
    /// <summary>
    /// Component to monitor Whisper types of events received from the client
    /// <para>Component based event handlers using UnityEvent for inspector access</para>
    /// </summary>
    public class MonitorWhisperEvents : MonoBehaviour
    {
        [SerializeField, RequiredInHierarchy(typeof(WhispersBehaviour))]
        WhispersBehaviour m_WhispersBehaviour = null;

        [SerializeField, Tooltip("Sends the entire whisper event args that was received.")]
        MonitorAnyWhispers m_OnAny = null;

        [SerializeField, Tooltip("Sends the nick and whisper and argument that was received.")]
        MonitorMessage m_OnMessage = null;

        /// <summary>
        /// Event invoked when whisper filter is processed by the monitor
        /// <para>Sends the nick and whisper that was received</para>
        /// </summary>
        public MonitorMessage onString { get { return m_OnMessage; } }

        void Start()
        {
            m_WhispersBehaviour.onReceived += (twitchChat, actionsArgs) =>
            {
                m_OnAny.Invoke(actionsArgs);

                if (!string.IsNullOrEmpty(actionsArgs.who))
                {
                    m_OnMessage.Invoke(actionsArgs.who, actionsArgs.message);
                }
            };
        }

    }
}
