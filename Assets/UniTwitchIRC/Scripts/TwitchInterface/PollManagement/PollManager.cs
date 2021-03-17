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

#pragma warning disable 0649

using IRCnect.Channel.Monitor;
using IRCnect.Channel.Monitor.Replies.Inbounds;
using System.Linq;
using System.Collections.Generic;
using UniTwitchIRC.Controllers;
using UnityEngine;
using System.Text.RegularExpressions;

namespace UniTwitchIRC.TwitchInterface.PollManagement
{
    /// <summary>
    /// Manages the stream polls
    /// </summary>
    [System.Serializable]
    public class PollManager : MonoBehaviour
    {

        static PollManager s_Instance = null;

        public static PollManager instance
        {
            get
            {
                if(s_Instance == null)
                {
                    s_Instance = FindObjectOfType<PollManager>();
                }
                return s_Instance;
            }
        }
        
        [SerializeField]
        bool m_OpenPoll = false;
        [SerializeField]
        bool m_SendPoll = true;
        [SerializeField]
        Statements m_Statements;

        [SerializeField]
        string m_FormatMessage = "{0} Choices - {1}";
        [SerializeField]
        string m_NumberSeperator = ")";
        [SerializeField]
        string m_Delimiter = " | ";

        [SerializeField]
        string m_Question = "Should we have a poll";

        [SerializeField]
        List<Option> m_Options = new List<Option>()
        {
            new Option(){value = "Yes", votes = 0},
            new Option(){value = "No", votes = 0},
            new Option(){value = "Maybe", votes = 0}
        };

        List<string> m_UserAnswered = new List<string>();
        List<InboundsFilter> m_InboundsFilters = new List<InboundsFilter>();
        
        AdminReference m_AdminReference = null;
        bool m_LastOpenPollState = false;

        public bool openPoll { get => m_OpenPoll; set => m_OpenPoll = value; }

        void Start()
        {
            m_AdminReference = GameObject.FindObjectOfType<AdminReference>();

            AddMonitorFilters();
        }

        void FixedUpdate()
        {
            if (m_SendPoll == false) return;

            if(m_OpenPoll != m_LastOpenPollState)
            {
                if(m_OpenPoll == true)
                {
                    m_UserAnswered.Clear();
                    for (int i = 0; i < m_Options.Count; i++)
                    {
                        Option tempOption = m_Options[i];
                        tempOption.votes = 0;
                        m_Options[i] = tempOption;
                    }

                    if (string.IsNullOrEmpty(m_Statements.opened) == false)
                    {
                        m_AdminReference.twitchChat.SendChatMessage(m_Statements.opened);
                    }
                    string pollMessage = string.Format(m_FormatMessage, m_Question, string.Join(m_Delimiter, m_Options.Select((x, i) => $"{i + 1}{m_NumberSeperator} {x.value}").ToArray()));
                    m_AdminReference.twitchChat.SendChatMessage(pollMessage);

                    PollEvents.instance.InvokStateChanged(new StateChangeEventArgs(m_Options, PollState.OPENED));
                }
                else
                {
                    if (string.IsNullOrEmpty(m_Statements.closed) == false)
                    {
                        m_AdminReference.twitchChat.SendChatMessage(m_Statements.closed);
                    }

                    PollEvents.instance.InvokStateChanged(new StateChangeEventArgs(m_Options, PollState.CLOSED));

                    string[] totalMessages = m_Options.Select(x => $"{x.value} has {x.votes}").ToArray();
                    string totalMessage = string.Join(m_Delimiter, totalMessages);
                    m_AdminReference.twitchChat.SendChatMessage(totalMessage);
                }
            }
            m_LastOpenPollState = m_OpenPoll;
        }

        void InvokeOnReceived(MonitorArgs obj)
        {
            if (m_OpenPoll == false) return;

            InboundsArgs e = obj as InboundsArgs;

            if (m_UserAnswered.Contains(e.nick)) return;

            m_UserAnswered.Add(e.nick);

            int index = (int.Parse(e.said)) - 1;

            Option option = m_Options[index];

            option.votes++;

            m_Options[index] = option;

            PollEvents.instance.InvokeVoteChanged(new VoteChangeEventArgs(option, e));
        }

        void AddMonitorFilters()
        {
            m_AdminReference.twitchChat.RemoveMonitorFilters(m_InboundsFilters.ToArray());
            m_InboundsFilters.Clear();
            for (int i = 1; i < m_Options.Count + 1; i++)
            {
                int index = i - 1;
                Option tempOption = m_Options[index];
                tempOption.index = index;
                m_Options[index] = tempOption;
                m_InboundsFilters.Add(((InboundsFilter)new InboundsFilter()
                    .AddRepliesFilter(new[] { i.ToString() }, InvokeOnReceived)));
            }
            m_AdminReference.twitchChat.AddMonitorFilters(m_InboundsFilters.ToArray());
        }

        public void CreatePoll(string question, IEnumerable<Option> options)
        {
            m_Question = question;
            m_Options = options.ToList();
            AddMonitorFilters();
        }
    }
}
#pragma warning restore 0649