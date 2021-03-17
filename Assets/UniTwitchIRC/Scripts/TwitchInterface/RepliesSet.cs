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

using System.Collections.Generic;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// The parsed parts of the message received from the client
    /// <para>Filtered by the help provider, HelpController, for providing help to the the Twitch chat client</para>
    /// </summary>
    [System.Serializable]
    public class RepliesSet
    {
        /// <summary>
        /// Expected method delegate for replied you expecet to received
        /// </summary>
        /// <param name="repliesSet">The collection of the replies set you expected.</param>
        public delegate void Added(IEnumerable<RepliesSet> repliesSet);

        [SerializeField]
        string m_Message = string.Empty;

        [SerializeField]
        string m_Description = string.Empty;

        /// <summary>
        /// The command or greeting or any inbounds message you specify
        /// </summary>
        public string message { get { return m_Message; } }

        /// <summary>
        /// The description of the inbounds message command or greeting
        /// </summary>
        public string description { get { return m_Description; } }
    }
}