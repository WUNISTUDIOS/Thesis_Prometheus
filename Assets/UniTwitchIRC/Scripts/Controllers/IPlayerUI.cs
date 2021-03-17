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
namespace UniTwitchIRC.Controllers
{
    /// <summary>
    /// Interface to get the default player controller and set it to the required controller
    /// </summary>
    public interface IPlayerUI
    {
        /// <summary>
        /// Sets the player controller to the desired controller
        /// </summary>
        TwitchPlayerController playerController { set; }
    }
}
