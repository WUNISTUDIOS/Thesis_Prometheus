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

using UniTwitchIRC.TwitchInterface;
using UnityEngine;

namespace UniTwitchIRC.Controllers
{
    /// <summary>
    /// Main reference for help and Inbounds events on the client
    /// </summary>
    public class AdminReference : MonoBehaviour
    {
        [SerializeField, RequiredInHierarchy(typeof(TwitchChat))]
        TwitchChat m_TwitchChat = null;

        /// <summary>
        /// Access to the Twitch IRC Chat client
        /// </summary>
        public TwitchChat twitchChat { get { return m_TwitchChat; } }
    }
}