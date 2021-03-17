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
using System.Collections;
using TwitchUnityIRC.Channel.Monitor;
using TwitchUnityIRC.Connection;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// The Co Monitor uses a Coroutine to monitor the background thread client messages
    /// </summary>
    public class TwitchCoMonitor : TwitchMonitor
    {
        protected volatile string m_Message = null;

        /// <summary>
        /// Disable the monitor permanently.  Once disabled this moniotor cannot be restarted without starting the Coroutine again.
        /// <para>Use running to stop and start the monitor if disabling is not your intent.</para>
        /// </summary>
        public bool enabled { get; set; }

        /// <summary>
        /// Is the monitor currently running
        /// <para>Starts and stops the monitoring.</para>
        /// </summary>
        public bool running { get; set; }

        /// <summary>
        /// Gets the filters set on the monitor to filter and capture specific inbound messages.
        /// </summary>
        public MonitorFilter[] filters { get { return base.Filters; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client to use for this monitor</param>
        public TwitchCoMonitor(TwitchChatClient client)
            : base(client)
        { }

        /// <summary>
        /// Methos for direct message insert into monitor for processing
        /// </summary>
        /// <param name="message">Fully parsable IRC message</param>
        /// <returns></returns>
        public bool TryInjectMessage(string message)
        {
            if(string.IsNullOrEmpty(m_Message)) {
                m_Message = message;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Monitor the chat client on the background thread
        /// </summary>
        public override void Monitor()
        {
            string message;
            if(TryGet(out message))
            {
                if(!ConsumeMessage(message))
                {
                    if(LogMonitor != null)
                    {
                        LogMonitor.Invoke(message);
                    }
                    m_Message = message;
                }
            }
        }

        /// <summary>
        /// Receives the chat messages from the background thread monitor
        /// </summary>
        /// <returns></returns>
        public IEnumerator Runner()
        {
            running = true;
            enabled = true;
            while(enabled)
            {
                if(running)
                {
                    if(m_Message != null)
                    {
                        foreach(var args in Parse(m_Message))
                        {
                            InvokeRecieved(args);
                        }
                        m_Message = null;
                    }
                }
                yield return null;
            }
        }
    }

}
