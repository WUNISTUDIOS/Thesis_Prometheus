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
using IRCnect.Channel.Monitor.Replies.Inbounds;
using System.Collections.Generic;
using UniTwitchIRC;
using UniTwitchIRC.Controllers;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// List to enter to greetings expected
    /// <para>All greetings you use should be entered here for global access to the parameters</para>
    /// </summary>
    [RequireComponent(typeof(AdminReference))]
    public class GreetingsBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Event to invoke when a greeting has been added to the internal list from the start method
        /// </summary>
        public static event RepliesSet.Added OnGreetingsAdded;

        /// <summary>
        /// Event invoked when a greeting is received from the client
        /// </summary>
        public static event System.Action<TwitchChat, InboundsArgs> OnGreetingsReceived;

        [SerializeField]
        List<RepliesSet> m_Greetings = null;

        List<string> m_Greeted = null;

        AdminReference m_AdminReference = null;

        /// <summary>
        /// Event to invoke when a greeting has been added to the internal list from the start method
        /// </summary>
        public event RepliesSet.Added onAdded = (rs) => { };

        /// <summary>
        /// Event invoked when a greeting is received from the client
        /// </summary>
        public event System.Action<TwitchChat, InboundsArgs> onReceived = (tw, ca) => { };

        /// <summary>
        /// Iterater over all the greeting arrays and adds them to the inbounds filters callback request monitor
        /// <para>Override in derives classes to provide addition functionality</para>
        /// </summary>
        protected virtual void Start()
        {
            m_AdminReference = GetComponent<AdminReference>();

            foreach(var greet in m_Greetings)
            {
                m_AdminReference.twitchChat.AddMonitorFilters(new InboundsFilter()
                    .AddBasicGreetings(new[] { greet.message }, InvokeOnReceived));
            }

            InvokeOnAdded(m_Greetings);

            m_Greeted = new List<string>();

        }

        void InvokeOnAdded(IEnumerable<RepliesSet> repliesSet)
        {
            onAdded.Invoke(repliesSet);
            if(GreetingsBehaviour.OnGreetingsAdded != null)
            {
                GreetingsBehaviour.OnGreetingsAdded.Invoke(repliesSet);
            }
        }

        void InvokeOnReceived(MonitorArgs obj)
        {
            InboundsArgs e = obj as InboundsArgs;
            onReceived.Invoke(m_AdminReference.twitchChat, e);

            GreetingsBehaviour.OnGreetingsReceived?.Invoke(m_AdminReference.twitchChat, e);

            if (m_Greeted.Contains(e.nick)) return;
            m_Greeted.Add(e.nick);

            var greetingSet = m_Greetings.Find(x => string.Compare(x.message, e.greeting, true) == 0);

            if(greetingSet.description.Contains("{0}") && greetingSet.description.Contains("{1}"))
            {
                m_AdminReference.twitchChat.SendChatMessageFormat(greetingSet.description, e.nick, e.greeting);
            }
            else if(greetingSet.description.Contains("{0}"))
            {
                m_AdminReference.twitchChat.SendChatMessageFormat(greetingSet.description, e.nick);
            }
            else
            {
                m_AdminReference.twitchChat.SendChatMessage(greetingSet.description);
            }
        }
    }
}