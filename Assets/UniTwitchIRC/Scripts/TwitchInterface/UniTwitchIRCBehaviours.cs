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
using System.Diagnostics;
using UniTwitchIRC;
using UniTwitchIRC.Controllers;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Host the basic components required to use UniTwitch IRC
    /// </summary>
    public class UniTwitchIRCBehaviours : MonoBehaviour
    {
        [System.Obsolete("Discontiued the chat popup support", true)]
        [SerializeField, HideInInspector]
        bool m_OpenOnRun = false;

        /// <summary>
        /// Opens the chat popup when playmode starts
        /// </summary>
        [System.Obsolete("Discontiued the chat popup support", true)]
        public bool openOnRun { get { return m_OpenOnRun; } }

    }
}