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
    /// Component to monitor Commands types of events received from the client
    /// <para>Component based event handlers using UnityEvent for inspector access</para>
    /// </summary>
    public class MonitorCommandsEvents : MonoBehaviour
    {

        [SerializeField, RequiredInHierarchy(typeof(CommandsBehaviour))]
        CommandsBehaviour m_CommandsBehaviour = null;

        [SerializeField, Tooltip("Sends the entire commands event args that was received.")]
        MonitorAnyCommands m_OnAny = null;

        [SerializeField, Tooltip("Sends the nick and command that was received.")]
        MonitorCommand m_OnCommand = null;

        [SerializeField, Tooltip("Sends the nick and command and argument that was received.")]
        MonitorParam m_OnArgument = null;

        [SerializeField, Tooltip("Sends the nick and command and argument array list that was received.")]
        MonitorNArgunment m_OnNArgunment = null;

        /// <summary>
        /// Event invoked when any commands filter is processed by the monitor
        /// <para>Sends the entire commands event args that was received</para>
        /// </summary>
        public MonitorAnyCommands onAny { get { return m_OnAny; } }

        /// <summary>
        /// Event invoked when commands filter is processed by the monitor
        /// <para>Sends the nick and command that was received</para>
        /// </summary>
        public MonitorCommand onCommand { get { return m_OnCommand; } }

        /// <summary>
        /// Event invoked when commands filter is processed by the monitor
        /// <para>Sends the nick and command and argument that was received</para>
        /// </summary>
        public MonitorParam onArgument { get { return m_OnArgument; } }

        /// <summary>
        /// Event invoked when commands filter is processed by the monitor
        /// <para>Sends the nick and command and argument array list that was received</para>
        /// </summary>
        public MonitorNArgunment onNArgunment { get { return m_OnNArgunment; } }

        void Start()
        {
            m_CommandsBehaviour.onReceived += (twitchChat, commandsArgs) =>
            {
                m_OnAny.Invoke(commandsArgs);
                m_OnCommand.Invoke(commandsArgs.nick, commandsArgs.command);
                m_OnArgument.Invoke(commandsArgs.nick, commandsArgs.command, commandsArgs.argument);
                m_OnNArgunment.Invoke(commandsArgs.nick, commandsArgs.command, commandsArgs.nArgument);
            };
        }
    }
}
