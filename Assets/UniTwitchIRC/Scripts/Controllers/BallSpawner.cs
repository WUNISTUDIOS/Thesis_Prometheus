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

using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using UniTwitchIRC.TwitchInterface;
using UnityEngine;

namespace UniTwitchIRC.Controllers
{
    /// <summary>
    /// Spawn an item for a viewer from a chat message command
    /// </summary>
    public sealed class BallSpawner : TwitchSpawner
    {
        /// <summary>
        /// Gives derived classes an oppotunity to modify the GameObject
        /// </summary>
        /// <param name="playerController">Player controller created</param>
        /// <param name="playerGameObject">Player associated GameObject created</param>
        protected override void PlayerPrefabInstantiated(TwitchPlayerController playerController, GameObject playerGameObject)
        {
            base.PlayerPrefabInstantiated(playerController, playerGameObject);

            IPlayerUI playerUI = playerGameObject.GetComponent<IPlayerUI>();
            if(playerUI != null)
            {
                playerGameObject.GetComponent<IPlayerUI>().playerController = playerController;
                playerGameObject.name = string.Concat(playerGameObject.name, ".", playerController.nick);
            }
        }
    }
}
