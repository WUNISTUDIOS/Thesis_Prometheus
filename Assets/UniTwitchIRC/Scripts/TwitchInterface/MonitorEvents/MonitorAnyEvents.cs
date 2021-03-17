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
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface.MonitorEvents
{
    /// <summary>
    /// Component to monitor all types of events received from the client
    /// <para>Component based event handlers using UnityEvent for inspector access</para>
    /// </summary>
    public class MonitorAnyEvents : MonoBehaviour
    {

        [SerializeField, Tooltip("Sends the entire event args that was received.")]
        MonitorAnyEvent m_OnAny = null;

        [SerializeField, Tooltip("Sends a string representation of the raw message that was received.")]
        MonitorRawData m_OnRawData = null;

        /// <summary>
        /// Event invoked when any filter is processed by the monitor
        /// <para>Sends the entire event args that was received</para>
        /// </summary>
        public MonitorAnyEvent onAny { get { return m_OnAny; } }

        /// <summary>
        /// Event invoked when any filter is processed by the monitor
        /// <para>Sends a string representation of the raw message that was received</para>
        /// </summary>
        public MonitorRawData onRawData { get { return m_OnRawData; } }

        void OnEnable()
        {
            TwitchCoMonitor.OnReceived -= TwitchChat_OnReceived;
            TwitchCoMonitor.OnReceived += TwitchChat_OnReceived;
        }

        void OnDisable()
        {
            TwitchCoMonitor.OnReceived -= TwitchChat_OnReceived;
        }

        void TwitchChat_OnReceived(object sender, MonitorArgs e)
        {
            m_OnAny.Invoke(e);
            m_OnRawData.Invoke(e.data);
        }
    }
}
