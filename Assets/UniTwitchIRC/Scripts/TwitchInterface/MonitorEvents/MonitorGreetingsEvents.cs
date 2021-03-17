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
    /// Component to monitor Greetings types of events received from the client
    /// <para>Component based event handlers using UnityEvent for inspector access</para>
    /// </summary>
    public class MonitorGreetingsEvents : MonoBehaviour
    {
        [SerializeField, RequiredInHierarchy(typeof(GreetingsBehaviour))]
        GreetingsBehaviour m_GreetingsBehaviour = null;

        [SerializeField, Tooltip("Sends the entire inbounds/greeting event args that was received.")]
        MonitorAnyInbounds m_OnAny = null;

        [SerializeField, Tooltip("Sends only the greeting that was received.")]
        MonitorGreet m_OnGreet = null;

        [SerializeField, Tooltip("Sends the nick and greeting that was received.")]
        MonitorNickSaid m_OnNickSaid = null;

        /// <summary>
        /// Event invoked when any inbounds/greeting filter is processed by the monitor
        /// <para>Sends the entire inbounds/greeting event args that was received</para>
        /// </summary>
        public MonitorAnyInbounds onAny { get { return m_OnAny; } }

        /// <summary>
        /// Event invoked when commands filter is processed by the monitor
        /// <para>Sends only the greeting that was received</para>
        /// </summary>
        public MonitorGreet onGreet { get { return m_OnGreet; } }

        /// <summary>
        /// Event invoked when commands filter is processed by the monitor
        /// <para>Sends the nick and greeting that was received</para>
        /// </summary>
        public MonitorNickSaid onNickSaid { get { return m_OnNickSaid; } }

        void Start()
        {
            m_GreetingsBehaviour.onReceived += (twitchChat, inboundsArgs) => 
            {
                m_OnAny.Invoke(inboundsArgs);
                m_OnGreet.Invoke(inboundsArgs.greeting);
                m_OnNickSaid.Invoke(inboundsArgs.nick, inboundsArgs.said);
            };
        }
    }
}
