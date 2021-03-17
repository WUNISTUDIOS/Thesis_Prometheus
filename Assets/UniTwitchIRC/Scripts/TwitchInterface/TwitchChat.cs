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

using IRCnect.Channel.Interaction;
using IRCnect.Channel.Monitor;
using IRCnect.Workers;
using PasswordProtector;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TwitchUnityIRC.Channel;
using TwitchUnityIRC.Channel.Interaction;
using TwitchUnityIRC.Connection;
using TwitchUnityIRC.Workers;
using UnityEngine;
using IRCnect.Channel.Monitor.Replies.Actions;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// The is the main connection component to the Twitch chat client
    /// </summary>
    public class TwitchChat : MonoBehaviour
    {
        [PasswordProtect]
        [SerializeField]
        protected string m_OAuth = string.Empty;

        [SerializeField, Tooltip("This is the nick for another the user or bot that will monitor the chat room.\nWARNING: Can not be the same as the channel/broadcaster name!")]
        protected string m_Nick = string.Empty;

        [SerializeField, Tooltip("This is the name of the channel, the broadcaster of the chat room.")]
        protected string m_Channel = string.Empty;

        [SerializeField, Space()]
        string m_JoinRoomMessage = "Hi chat!";

        protected TwitchChatClient m_Client = null;
        protected TwitchMessenger m_Messenger = null;
        protected TwitchCoMonitor m_Monitor = null;
        protected TwitchRoomVisitor m_RoomVisitor = null;
        IWorker m_Worker = null;

        Queue<PendingMessageArgs> m_PendingMessageQueue = null;

        [SerializeField, HideInInspector]
        bool m_HideRunInBackgroundMessage = false;
        
        public void InjectMessage(string nick, string channel, string message)
        {
            string fullMessage = string.Format("@empty-meta=; :{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :{2}", nick, channel, message);
            m_Monitor.TryInjectMessage(fullMessage);
        }

        /// <summary>
        /// Indicates that the run in background message show be hidden in the inspector
        /// <para>Used by the Editor script</para>
        /// </summary>
        public bool HideRunInBackgroundMessage { get { return m_HideRunInBackgroundMessage; } }

        /// <summary>
        /// Add subscribers to the monitor received event
        /// <para>You can monitor arr communications this way.</para>
        /// </summary>
        public event EventHandler<MonitorArgs> onReceived
        {
            add { CheckForNullMonitor(); m_Monitor.onReceived += value; }
            remove { CheckForNullMonitor(); m_Monitor.onReceived -= value; }
        }
        /// <summary>
        /// The client with the stream reader and writer for the chat connection
        /// </summary>
        public TwitchChatClient client { get { return m_Client; } }

        /// <summary>
        /// The object representing the nick for another the user or bot that will monitor the chat room
        /// </summary>
        public TwitchRoomVisitor roomVisitor { get { return m_RoomVisitor; } }

        /// <summary>
        /// Messenger providing the functionality for sending messages along the IRC stream.
        /// </summary>
        public TwitchMessenger messenger { get { return m_Messenger; } }

        /// <summary>
        /// Interface for monitoring IRC inbound stream reader
        /// </summary>
        public TwitchCoMonitor monitor { get { return m_Monitor; } }

        /// <summary>
        /// Worker interface to mainly handle Monitor reading.
        /// </summary>
        public IWorker worker { get { return m_Worker; } }

        /// <summary>
        /// Event invoked when a monitor filter is added to the filter list
        /// </summary>
        public event Action<MonitorFilter[]> onFilterAdded;

        /// <summary>
        /// Event invoked when a monitor filter is removed  to the filter list
        /// </summary>
        public event Action<MonitorFilter[]> onFilterRemoved;

        /// <summary>
        /// Called when the scene starts
        /// <para>Opens the connection and starts the Coroutine and Thread to monitor the IRC client</para>
        /// </summary>
        protected void Awake()
        {
            m_Client = new TwitchChatClient();

            OpenConnection();

            StartChatMonitor();

            StartCoroutine(m_Monitor.Runner());

            EnableChatMessenger();

            Capabilities(TwitchProtocol.CAP_REQ.All);
            Connect();
        }

        IEnumerator Start()
        {
            while(enabled)
            {
                if(m_PendingMessageQueue.Count > 0)
                {
                    if(m_Messenger.CanSendMessage)
                    {
                        PendingMessageArgs pendingMessageArgs = m_PendingMessageQueue.Dequeue();

                        switch (pendingMessageArgs.type)
                        {
                            case PendingMessageArgs.Type.Chat:
                                SendChatMessage(pendingMessageArgs.message, pendingMessageArgs.args);
                                break;
                            case PendingMessageArgs.Type.Whisper:
                                string message = pendingMessageArgs.message;
                                int length = message.IndexOf(' ');
                                string toNick = message.Substring(0, length);
                                string toMessage = message.Substring(toNick.Length);
                                SendWhisperMessage(toNick, toMessage);
                                break;
                            default:
                                break;
                        }
                    }
                }
                yield return new WaitForSeconds((float)m_Messenger.SendDelaySeconds);
            }
        }

        void OnEnable()
        {
            JoinRoom();
            if(!string.IsNullOrEmpty(m_JoinRoomMessage))
            {
                SendChatMessage(m_JoinRoomMessage);
            }
        }

        void OnDisable()
        {
            PartRoom();
        }

        void OnDestroy()
        {
            m_Monitor.running = false;
            StopChatMonitor();
            CloseConnection();
        }

        void CheckForNullMonitor()
        {
            if(m_Monitor == null)
            {
                throw new NullReferenceException("Monitor can not be null call OpenConnection() first then call StartChatMonitor(IWorker).");
            }
        }

        void CheckForNullMessenger()
        {
            if(m_Messenger == null)
            {
                throw new NullReferenceException("Messenger can not be null call EnableChatMessenger() first then messages can be sent.");
            }
        }

        /// <summary>
        /// Add a monitor filter to the internal list
        /// </summary>
        /// <param name="filters">Filters to add</param>
        /// <returns>The monitor that the filter ead added to</returns>
        public MonitorBase AddMonitorFilters(params MonitorFilter[] filters)
        {
            if (onFilterAdded != null)
            {
                onFilterAdded.Invoke(filters);
            }
            return m_Monitor.AddFilters(filters);
        }

        /// <summary>
        /// Add a monitor filter to the internal list
        /// </summary>
        /// <param name="filters">Filters to remove</param>
        /// <returns>The monitor that the filter ead added to</returns>
        public MonitorBase RemoveMonitorFilters(params MonitorFilter[] filters)
        {
            if (onFilterRemoved != null)
            {
                onFilterRemoved.Invoke(filters);
            }
            return m_Monitor.RemoveFilters(filters);
        }

        /// <summary>
        /// Enables the chat messenger to allow for sending messages
        /// <para>Called once on startup in the Awake method</para>
        /// </summary>
        public virtual void EnableChatMessenger()
        {
            m_PendingMessageQueue = new Queue<PendingMessageArgs>();

            m_Messenger = new TwitchMessenger(m_RoomVisitor, m_Channel);
            m_Messenger.onMessagePending += Messenger_onMessagePending;
        }

        void Messenger_onMessagePending(object sender, PendingMessageArgs args)
        {
            m_PendingMessageQueue.Enqueue(args);
        }

        /// <summary>
        /// Opens the chat client connection
        /// <para>Called once on startup in the Awake method</para>
        /// </summary>
        public virtual void OpenConnection()
        {
            m_Client.OpenConnection(TwitchProtocol.HOSTNAME, TwitchProtocol.PORT_CHAT);//"asimov.freenode.net""irc.freenode.net"
            m_RoomVisitor = new TwitchRoomVisitor(m_Nick, m_Client.writer);
            m_Monitor = new TwitchCoMonitor(m_Client);
        }

        /// <summary>
        /// Connects to the IRC client once the connection has been opened
        /// <para>Called once on startup in the Awake method</para>
        /// </summary>
        public void Connect()
        {
            m_RoomVisitor.Connect(m_OAuth);
        }

        /// <summary>
        /// Sends capabilities requests to server
        /// <para>NOTE: MUST be sent before joining the channel and after Connect is called.</para>
        /// </summary>
        /// <param name="capReqs">Use one of TwitchProtocol.CAP_REQ in the Utils namespace.
        /// <para>Leave without parameters to use TwitchProtocol.CAP_REQ.All</para></param>
        public void Capabilities(params string[] capReqs)
        {
            m_RoomVisitor.Capabilities(capReqs);

        }

        /// <summary>
        /// Close the connection to the chat client
        /// <para>Called once in OnDestroy</para>
        /// </summary>
        public void CloseConnection()
        {
            if(m_Worker != null)
            {
                throw new InvalidOperationException("The chat monitor is still running call StopChatMonitor() before trying to close the connection.");
            }
            m_Client.CloseConnection();
        }

        /// <summary>
        /// Call to join a specific channel or the default connection channel
        /// <para>NOTE: Rejoins on scene change</para>
        /// <para>Called once in OnEnable</para>
        /// </summary>
        /// <param name="channel">Channel to join</param>
        public void JoinRoom(string channel = null)
        {
            m_RoomVisitor.JoinRoom(string.IsNullOrEmpty(channel) ? m_Channel : channel);
        }

        /// <summary>
        /// Call to leave/part a chat room
        /// <para>NOTE: Leaves on scene change</para>
        /// <para>Called once in OnDisable</para>
        /// </summary>
        /// <param name="channel"></param>
        public void PartRoom(string channel = null)
        {
            m_RoomVisitor.PartRoom(string.IsNullOrEmpty(channel) ? m_Channel : channel);
        }

        /// <summary>
        /// Send a string format message to the chat room
        /// <para>Safe send using a timer throttle</para>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="args">Format args</param>
        public void SendChatMessageFormat(string message, params object[] args)
        {
            if(args.Length == 0)
            {
                Debug.LogError("There are no arguments, use Send(string message) instead.");
                return;
            }
            this.SendChatMessage(string.Format(message, args));
        }

        /// <summary>
        /// Send a string format message to the chat room
        /// <para>UsSafe send ignoring the timer throttle</para>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="args">Format args</param>
        public void SendChatMessageFormatUnsafe(string message, params object[] args)
        {
            if(args.Length == 0)
            {
                Debug.LogError("There are no arguments, use SendUnsafe(string message) instead.");
                return;
            }
            this.SendChatMessageUnsafe(string.Format(message, args));
        }

        /// <summary>
        /// Send a string format message to the chat room
        /// <para>Safe send using a timer throttle</para>
        /// </summary>
        /// <param name="message">Message to send</param>
        public void SendChatMessage(string message)
        {
            CheckForNullMessenger();
            m_Messenger.Send(PendingMessageArgs.Type.Chat, message);
        }

        /// <summary>
        /// Send a string format message to the chat room
        /// <para>Safe send using a timer throttle</para>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="args">Message string format arguments</param>
        public void SendChatMessage(string message, object[] args)
        {
            CheckForNullMessenger();
            m_Messenger.Send(PendingMessageArgs.Type.Chat, message);
        }

        /// <summary>
        /// Send a string format message to the chat room
        /// <para>UsSafe send ignoring the timer throttle</para>
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="args">Message string format arguments</param>
        public void SendChatMessageUnsafe(string message, object[] args = null)
        {
            CheckForNullMessenger();
            m_Messenger.SendUnsafe(PendingMessageArgs.Type.Chat, message);
        }

        /// <summary>
        /// Send a string format message to the chat room
        /// <para>Safe send using a timer throttle</para>
        /// </summary>
        /// <param name="toNick">To who</param>
        /// <param name="message">Message to send</param>
        public void SendWhisperMessage(string toNick, string message, object[] args = null)
        {
            CheckForNullMessenger();
            m_Messenger.Send(PendingMessageArgs.Type.Whisper, string.Format("{0} {1}", toNick, message));
        }

        /// <summary>
        /// Send a string format message to the chat room
        /// <para>UsSafe send ignoring the timer throttle</para>
        /// </summary>
        /// <param name="toNick">To who</param>
        /// <param name="message">Message to send</param>
        /// <param name="args">Message string format arguments</param>
        public void SendWhisperMessageUnsafe(string toNick, string message, object[] args = null)
        {
            CheckForNullMessenger();
            m_Messenger.SendUnsafe(PendingMessageArgs.Type.Whisper, string.Format("{0} {1}", toNick, message));
        }

        /// <summary>
        /// Start chat monitor to allow for monitoring the IRC incoming messages
        /// <para>Called once on startup in the Awake method</para>
        /// </summary>
        public void StartChatMonitor(IWorker preferedWorker = null)
        {
            if(m_Worker != null) return;
            CheckForNullMonitor();
            m_Worker = preferedWorker ?? new TwitchWorker(m_Monitor.Monitor);
            m_Worker.Run();
        }

        class LocalWorker : Worker
        {
            MonoBehaviour m_MonoBehaviour = null;
            Action m_ToExecute = null;

            public LocalWorker(MonoBehaviour monoBehaviour, Action toExecute) : base()
            {
                m_MonoBehaviour = monoBehaviour;
                m_ToExecute = toExecute;
            }

            public override void Run()
            {
                m_MonoBehaviour.StartCoroutine(Runner());
            }
            
            IEnumerator Runner()
            {
                IsRunning = true;
                while (IsRunning)
                {
                    m_ToExecute.Invoke();
                    yield return null;
                }
            }

        }

        /// <summary>
        /// Stops monitoring the chat room
        /// <para>Called once in OnDestroy</para>
        /// </summary>
        public void StopChatMonitor()
        {
            if(m_Worker == null) return;
            m_Worker.Stop();
            m_Worker = null;
        }

    }
}
