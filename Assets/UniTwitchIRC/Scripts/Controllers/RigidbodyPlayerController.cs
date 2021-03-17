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
using System;
using UniTwitchIRC.TwitchInterface;
using UnityEngine;

namespace UniTwitchIRC.Controllers
{
    /// <summary>
    /// Allows the viewer to control the player from the Twitch chat room
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public sealed class RigidbodyPlayerController : TwitchPlayerController
    {
        Rigidbody m_Rigidbody;

        /// <summary>
        /// Call to initialize the Rigidbody component
        /// <para>Override in derived calsses to add functionality</para>
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Sets movement vector to zero
        /// <para>Override in derived calsses to add functionality</para>
        /// </summary>
        /// <param name="commandsArgs">Commands args received from tyhe chat client</param>
        protected override void TwitchChat_OnResetCommand(CommandsArgs commandsArgs)
        {
            base.TwitchChat_OnResetCommand(commandsArgs);
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.position = base.m_StartPosition;
        }

        /// <summary>
        /// Sends the movement vectors to the derived classes
        /// <para>Override in derived calsses to add functionality</para>
        /// </summary>
        /// <param name="movement">Clamped movement vector, movement clamped to max speed.</param>
        /// <param name="rawMovement">Unclamped movement vector</param>
        protected override void OnMoveCommandReceived(Vector3 movement, Vector3 rawMovement)
        {
            m_Rigidbody.AddForce(movement, ForceMode.Impulse);
        }

        /// <summary>
        /// <para>Override in derived calsses to add functionality</para>
        /// </summary>
        /// <param name="nick">The nick for the player controller</param>
        /// <param name="startPosition">Required start position</param>
        public override void StartNew(string nick, Vector3 startPosition)
        {
            base.StartNew(nick, startPosition);
            m_Rigidbody.position = startPosition;
        }
    }
}
