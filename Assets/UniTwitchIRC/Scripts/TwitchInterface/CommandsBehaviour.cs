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
using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using System.Collections.Generic;
using System.Linq;
using UniTwitchIRC.Controllers;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// List to enter to commands expected
    /// <para>All commands you use should be entered here for global access to the parameters</para>
    /// </summary>
    [RequireComponent(typeof(AdminReference))]
    public class CommandsBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Event to invoke when a command has been added to the internal list from the start method
        /// </summary>
        public static event RepliesSet.Added OnCommandsAdded;

        /// <summary>
        /// Event invoked when a command is received from the client
        /// </summary>
        public static event System.Action<TwitchChat, CommandsArgs> OnCommandsReceived;

        [SerializeField]
        RepliesSet[] m_BasicCommands = null;

        [SerializeField]
        RepliesSet[] m_ParameterizedCommands = null;

        [SerializeField]
        RepliesSet[] m_NArgunmentCommands = null;

        AdminReference m_AdminReference = null;

        /// <summary>
        /// Event to invoke when a command has been added to the internal list from the start method
        /// </summary>
        public event RepliesSet.Added onAdded = (rs) => { };
        
        /// <summary>
        /// Event invoked when a command is received from the client
        /// </summary>
        public event System.Action<TwitchChat, CommandsArgs> onReceived = (tw, ca) => { };

        /// <summary>
        /// Concats all the commands array into one list
        /// </summary>
        /// <returns>The complete list of the commands</returns>
        public RepliesSet[] GetRepliesSets()
        {
            List<RepliesSet> repliesSets = new List<RepliesSet>();
            repliesSets.AddRange(m_BasicCommands);
            repliesSets.AddRange(m_ParameterizedCommands);
            repliesSets.AddRange(m_NArgunmentCommands);
            return repliesSets.ToArray();
        }

        /// <summary>
        /// Iterater over all the command arrays and adds them to the command filters callback request monitor
        /// <para>Override in derives classes to provide addition functionality</para>
        /// </summary>
        protected virtual void Start()
        {
            m_AdminReference = GetComponent<AdminReference>();

            foreach(var command in m_BasicCommands)
            {
                m_AdminReference.twitchChat.AddMonitorFilters(new CommandsFilter()
                    .AddBasicCommand(command.message, InvokeOnReceived));
            }

            foreach(var command in m_ParameterizedCommands)
            {
                m_AdminReference.twitchChat.AddMonitorFilters(new CommandsFilter()
                    .AddParameterizedCommand(command.message, InvokeOnReceived));
            }

            foreach(var command in m_NArgunmentCommands)
            {
                m_AdminReference.twitchChat.AddMonitorFilters(new CommandsFilter()
                    .AddNParameterCommand(command.message, InvokeOnReceived));
            }

            InvokeOnAdded(m_BasicCommands);
            InvokeOnAdded(m_ParameterizedCommands);
            InvokeOnAdded(m_NArgunmentCommands);
        }

        void InvokeOnAdded(IEnumerable<RepliesSet> repliesSet)
        {
            onAdded.Invoke(repliesSet);
            if(CommandsBehaviour.OnCommandsAdded != null)
            {
                CommandsBehaviour.OnCommandsAdded.Invoke(repliesSet);
            }
        }

        void InvokeOnReceived(MonitorArgs obj)
        {
            CommandsArgs e = obj as CommandsArgs;
            onReceived.Invoke(m_AdminReference.twitchChat, e);
            if(CommandsBehaviour.OnCommandsReceived != null)
            {
                CommandsBehaviour.OnCommandsReceived(m_AdminReference.twitchChat, e);
            }
        }
    }
}