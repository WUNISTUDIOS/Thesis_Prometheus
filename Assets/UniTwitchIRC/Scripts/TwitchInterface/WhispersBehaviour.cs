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
using IRCnect.Channel.Monitor.Replies.Actions;
using UniTwitchIRC.Controllers;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// List to listen for whispers expected
    /// <para>All whispers you use should be entered here for global access to the parameters</para>
    /// </summary>
    [RequireComponent(typeof(AdminReference))]
    public class WhispersBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Event invoked when a whisper is received from the client
        /// </summary>
        public static event System.Action<TwitchChat, ActionsArgs> OnWhisperReceived;

        AdminReference m_AdminReference = null;

        /// <summary>
        /// Event invoked when a command is received from the client
        /// </summary>
        public event System.Action<TwitchChat, ActionsArgs> onReceived = (tw, ca) => { };

        /// <summary>
        /// Iterater over all the command arrays and adds them to the command filters callback request monitor
        /// <para>Override in derives classes to provide addition functionality</para>
        /// </summary>
        protected virtual void Start()
        {
            m_AdminReference = GetComponent<AdminReference>();

            m_AdminReference.twitchChat.AddMonitorFilters(new ActionsFilter().AddActionsFilters(new[] { ActionTypes.WHISPER }, InvokeOnReceived));
        }

        void InvokeOnReceived(MonitorArgs obj)
        {
            ActionsArgs e = obj as ActionsArgs;
            onReceived.Invoke(m_AdminReference.twitchChat, e);
            if (WhispersBehaviour.OnWhisperReceived != null)
            {
                WhispersBehaviour.OnWhisperReceived(m_AdminReference.twitchChat, e);
            }
        }
    }
}