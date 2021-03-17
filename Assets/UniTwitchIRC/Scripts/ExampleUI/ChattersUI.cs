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

using UnityEngine;
using UnityEngine.UI;

namespace UniTwitchIRC.ExampleUI
{
    /// <summary>
    /// Holds the text object reference for the viewer in the lobby
    /// </summary>
    public class ChattersUI : MonoBehaviour
    {
        /// <summary>
        /// Viewer/Player points text
        /// </summary>
        public Text pointsText = null;

        /// <summary>
        /// Chatter or viewer name text
        /// </summary>
        public Text chatterText = null;

        /// <summary>
        /// Time check for validating the viewer is still in the lobby
        /// </summary>
        public Text timerText = null;
    }
}
