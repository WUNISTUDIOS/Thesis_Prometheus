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
using System;

namespace UniTwitchIRC.TwitchInterface.PollManagement
{
    /// <summary>
    /// Manages the stream poll events
    /// </summary>
    [System.Serializable]
    public class PollEvents : MonoBehaviour
    {
        static PollEvents s_Instance = null;

        public static PollEvents instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<PollEvents>();
                }
                return s_Instance;
            }
        }

        [SerializeField]
        StateChangeEvent m_StateChangeEvent = null;
        [SerializeField]
        VoteChangeEvent m_VoteChangeEvent = null;

        internal void InvokStateChanged(StateChangeEventArgs stateChangeEventArgs)
        {
            m_StateChangeEvent.Invoke(stateChangeEventArgs);
        }

        internal void InvokeVoteChanged(VoteChangeEventArgs voteChangeEventArgs)
        {
            m_VoteChangeEvent.Invoke(voteChangeEventArgs);
        }
    }
}
#pragma warning restore 0649