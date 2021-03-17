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

using IRCnect.Channel.Monitor.Replies.Inbounds;
using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using System.Collections.Generic;
using System.Text;
using UniTwitchIRC.TwitchInterface;
using UnityEngine;

namespace UniTwitchIRC.Controllers
{
    /// <summary>
    /// Help controller provides help to the Twitch Chat room about the commands available within your application
    /// </summary>
    [RequireComponent(typeof(AdminReference))]
    public class HelpController : MonoBehaviour
    {
        [SerializeField, TwitchCommand, Tooltip("Gets avaliable help from the CommandsBehaviour list.")]
        string m_HelpCommand = "help";

        [SerializeField, TwitchCommand, Tooltip("Gets avaliable help for a specific command.")]
        string m_CommandHelp = "h";

        AdminReference m_AdminReference = null;

        StringBuilder m_InboundBuilder;
        StringBuilder m_CommandsBuilder;

        Dictionary<string, string> m_HelpDescriptions;

        void Awake()
        {
            m_HelpDescriptions = new Dictionary<string, string>();

            m_InboundBuilder = new StringBuilder("Greetings: ");
            m_CommandsBuilder = new StringBuilder("Commands: ");

            m_AdminReference = GetComponent<AdminReference>();
            m_AdminReference.twitchChat.onFilterAdded += (filters) =>
            {
                for(int i = 0; i < filters.Length; i++)
                {
                    foreach(var item in filters[i].rawInput)
                    {
                        if(filters[i] is CommandsFilter)
                        {
                            m_CommandsBuilder.Append(string.Concat(HelperExtensions.COMMAND_SYMBOL, item.Value));
                            m_CommandsBuilder.Append(", ");
                        }
                        else if(filters[i] is InboundsFilter)
                        {
                            m_InboundBuilder.Append(item.Value);
                            m_InboundBuilder.Append(", ");
                        }
                    }
                }
            };
        }

        void OnEnable()
        {
            CommandsBehaviour.OnCommandsAdded -= CommandsBehaviour_OnCommandsAdded;
            CommandsBehaviour.OnCommandsAdded += CommandsBehaviour_OnCommandsAdded;

            CommandsBehaviour.OnCommandsReceived -= CommandsBehaviour_OnCommandsReceived;
            CommandsBehaviour.OnCommandsReceived += CommandsBehaviour_OnCommandsReceived;
        }

        void OnDisable()
        {
            CommandsBehaviour.OnCommandsAdded -= CommandsBehaviour_OnCommandsAdded;

            CommandsBehaviour.OnCommandsReceived -= CommandsBehaviour_OnCommandsReceived;
        }

        void CommandsBehaviour_OnCommandsAdded(IEnumerable<RepliesSet> repliesSet)
        {
            foreach(var replies in repliesSet)
            {
                if(!m_HelpDescriptions.ContainsKey(replies.message))
                {
                    m_HelpDescriptions.Add(replies.message, replies.description);
                }
            }
        }

        void CommandsBehaviour_OnCommandsReceived(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            if(commandsArgs.IsCommand(m_HelpCommand))
            {
                string message = string.Concat(
                    m_InboundBuilder.ToString().Trim(new char[] { ',', ' ' }),
                    " - ",
                    m_CommandsBuilder.ToString().Trim(new char[] { ',', ' ' }));

                twitchChat.SendChatMessage(message);
            }
            else if(commandsArgs.IsCommand(m_CommandHelp))
            {
                if(m_HelpDescriptions.ContainsKey(commandsArgs.argument))
                {
                    string message = string.Concat("Help for ",
                        commandsArgs.argument, ": ",
                        m_HelpDescriptions[commandsArgs.argument]);
                    twitchChat.SendChatMessage(message);
                }
            }
        }
    }
}