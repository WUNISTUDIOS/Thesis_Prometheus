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
    /// Component to monitor Number types of events received from the client
    /// <para>Component based event handlers using UnityEvent for inspector access</para>
    /// </summary>
    public class MonitorNumberEvents : MonoBehaviour
    {
        [SerializeField, RequiredInHierarchy(typeof(CommandsBehaviour))]
        CommandsBehaviour m_CommandsBehaviour = null;

        [SerializeField, Tooltip("Sends the nick and command and int argument that was received.")]
        MonitorInt m_OnInt = null;

        [SerializeField, Tooltip("Sends the nick and command and float argument that was received.")]
        MonitorFloat m_OnFloat = null;

        [SerializeField, Tooltip("Sends the nick and command and int argument array list that was received.")]
        MonitorIntArray m_OnIntArray = null;

        [SerializeField, Tooltip("Sends the nick and command and float argument array list that was received.")]
        MonitorFloatArray m_OnFloatArray = null;

        [SerializeField, Tooltip("Sends the Twitch Chat object and entire commands event args that was received.")]
        MonitorTwitchCommand m_OnTwitchCommand = null;

        /// <summary>
        /// Event invoked when int number commands filter is processed by the monitor
        /// <para>Sends the nick and command and int argument that was received</para>
        /// </summary>
        public MonitorInt onIntCommand { get { return m_OnInt; } }

        /// <summary>
        /// Event invoked when float number commands filter is processed by the monitor
        /// <para>Sends the nick and command and float argument that was received</para>
        /// </summary>
        public MonitorFloat onFloatCommand { get { return m_OnFloat; } }

        /// <summary>
        /// Event invoked when int number commands filter is processed by the monitor
        /// <para>Sends the nick and command and int argument array list that was received</para>
        /// </summary>
        public MonitorIntArray onIntArrayCommand { get { return m_OnIntArray; } }

        /// <summary>
        /// Event invoked when float number commands filter is processed by the monitor
        /// <para>Sends the nick and command and float argument array list that was received</para>
        /// </summary>
        public MonitorFloatArray onFloatArrayCommand { get { return m_OnFloatArray; } }

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
                    if(!System.Array.TrueForAll(commandsArgs.nArgument, x => x.Contains("."))) //integer array check
                    {
                        int[] numbers = new int[commandsArgs.nArgument.Length];
                        for(int i = 0; i < commandsArgs.nArgument.Length; i++)
                        {
                            int.TryParse(commandsArgs.nArgument[i], out numbers[i]);
                        }
                        m_OnIntArray.Invoke(commandsArgs.nick, commandsArgs.command, numbers);
                    }
                    else //It's a float array
                    {
                        float[] numbers = new float[commandsArgs.nArgument.Length];
                        for(int i = 0; i < commandsArgs.nArgument.Length; i++)
                        {
                            float.TryParse(commandsArgs.nArgument[i], out numbers[i]);
                        }
                        m_OnFloatArray.Invoke(commandsArgs.nick, commandsArgs.command, numbers);
                    }
                }
                else if(!string.IsNullOrEmpty(commandsArgs.argument))
                {
                    if(commandsArgs.argument.Contains(".")) //float check
                    {
                        float number;
                        if(float.TryParse(commandsArgs.argument, out number))
                        {
                            m_OnFloat.Invoke(commandsArgs.nick, commandsArgs.command, number);
                        }
                    }
                    else //It's an integer
                    {
                        int number;
                        if(int.TryParse(commandsArgs.argument, out number))
                        {
                            m_OnInt.Invoke(commandsArgs.nick, commandsArgs.command, number);
                        }
                    }

                }
            };
        }

    }
}
