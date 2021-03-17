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
    /// Component to monitor String types of events received from the client
    /// <para>Component based event handlers using UnityEvent for inspector access</para>
    /// </summary>
    public class MonitorStringEvents : MonoBehaviour
    {
        [SerializeField, RequiredInHierarchy(typeof(CommandsBehaviour))]
        CommandsBehaviour m_CommandsBehaviour = null;

        [SerializeField, Tooltip("Sends the nick and command and argument that was received.")]
        MonitorString m_OnString = null;

        [SerializeField, Tooltip("Sends the nick and command and argument array list that was received.")]
        MonitorStringArray m_OnStringArray = null;

        [SerializeField, Tooltip("Sends the Twitch Chat object and entire commands event args that was received.")]
        MonitorTwitchCommand m_OnTwitchCommand = null;

        /// <summary>
        /// Event invoked when commands filter is processed by the monitor
        /// <para>Sends the nick and command and argument that was received</para>
        /// </summary>
        public MonitorString onString { get { return m_OnString; } }

        /// <summary>
        /// Event invoked when string commands filter is processed by the monitor
        /// <para>Sends the nick and command and argument that was received</para>
        /// </summary>
        public MonitorStringArray onStringArray { get { return m_OnStringArray; } }

        /// <summary>
        /// Event invoked when any filter is processed by the monitor
        /// <para>Sends the Twitch Chat object and entire commands event args that was received</para>
        /// </summary>
        public MonitorTwitchCommand onTwitchCommand { get { return m_OnTwitchCommand; } }

        void Start()
        {
            m_CommandsBehaviour.onReceived += (twitchChat, commandsArgs) =>
            {
                m_OnTwitchCommand.Invoke(twitchChat, commandsArgs);

                if(commandsArgs.nArgument.Length > 1)
                {
                    string[] strings = new string[commandsArgs.nArgument.Length];
                    for(int i = 0; i < commandsArgs.nArgument.Length; i++)
                    {
                        strings[i] = commandsArgs.nArgument[i];
                    }
                    m_OnStringArray.Invoke(commandsArgs.nick, commandsArgs.command, strings);
                }
                else if(!string.IsNullOrEmpty(commandsArgs.argument))
                {
                    m_OnString.Invoke(commandsArgs.nick, commandsArgs.command, commandsArgs.argument);
                }
            };
        }

    }
}
