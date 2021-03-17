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

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Allows the viewer to control the player from the Twitch chat room
    /// </summary>
    public abstract class TwitchPlayerController : MonoBehaviour
    {
        const int MX = 0, MY = 1, MZ = 2;

        [SerializeField, TwitchCommand]
        string m_ResetCommand = "reset";
        [SerializeField, TwitchCommand]
        string m_NameCommand = "name";
        [SerializeField, TwitchCommand]
        string m_MoveCommand = "move";
        [SerializeField, TwitchCommand]
        string m_XCommand = "x";
        [SerializeField, TwitchCommand]
        string m_YCommand = "y";
        [SerializeField, TwitchCommand]
        string m_ZCommand = "z";

        [SerializeField]
        string m_Nick = string.Empty;

        [SerializeField]
        Vector3 m_MaxSpeed = Vector3.one * 10.0f;

        Vector3 m_RawMovement = Vector3.zero;

        Vector3 m_Movement = Vector3.zero;

        /// <summary>
        /// Caches the transform start world position in the Awake method on start up
        /// </summary>
        protected Vector3 m_StartPosition = Vector3.zero;

        /// <summary>
        /// Gets/Sets the name of the player for this controller
        /// </summary>
        public string nick { get { return m_Nick; } set { m_Nick = value; } }

        /// <summary>
        /// Gets the player name to be displayed in your application
        /// <para>This is different from the nick in that it can be used to provide a different name for the viewer.</para>
        /// </summary>
        public string displayName { get; private set; }

        /// <summary>
        /// Gets/Sets points for the current player
        /// </summary>
        public int points { get; set; }

        /// <summary>
        /// Call to initialize components
        /// <para>Override in derived calsses to add functionality</para>
        /// </summary>
        protected virtual void Awake()
        {
            m_StartPosition = transform.position;
        }

        void OnValidate()
        {
            m_MaxSpeed.x = Mathf.Max(0, m_MaxSpeed.x);
            m_MaxSpeed.y = Mathf.Max(0, m_MaxSpeed.y);
            m_MaxSpeed.z = Mathf.Max(0, m_MaxSpeed.z);
        }

        internal virtual void OnDisplayNameChange(string nick, string command, string argument)
        {
            if(!nick.Equals(m_Nick)) return;
            if(!command.IsCommand(m_NameCommand)) return;

            this.displayName = argument;
        }

        internal void OnDisplayNameChange(string nick, string command, string[] nArgument)
        {
            OnDisplayNameChange(nick, command, string.Join(" ", nArgument));
        }

        internal void OnMoveUniDirection(string nick, string command, float argument)
        {
            if(!nick.Equals(m_Nick)) return;
            if(command.IsCommand(m_MoveCommand)) return;

            if(command.IsCommand(m_XCommand))
            {
                AddForceFromCommand(argument,
                    (val) => new Vector3(val, 0.0f, 0.0f),
                    (val) => Mathf.Clamp(val.x, -m_MaxSpeed.x, m_MaxSpeed.x));
            }
            else if(command.IsCommand(m_YCommand))
            {
                AddForceFromCommand(argument,
                    (val) => new Vector3(0.0f, val, 0.0f),
                    (val) => Mathf.Clamp(val.y, 0.0f, m_MaxSpeed.y));
            }
            else if(command.IsCommand(m_ZCommand))
            {
                AddForceFromCommand(argument,
                    (val) => new Vector3(0.0f, 0.0f, val),
                    (val) => Mathf.Clamp(val.z, -m_MaxSpeed.z, m_MaxSpeed.z));
            }
        }

        internal void OnMove(string nick, string command, float[] nArgument)
        {
            if(!nick.Equals(m_Nick)) return;
            if(!command.IsCommand(m_MoveCommand)) return;

            int requiredArgsCount = 3;
            if(nArgument.Length == requiredArgsCount)
            {
                MovePlayer(nArgument[MX], nArgument[MY], nArgument[MZ]);
            }
        }

        /// <summary>
        /// Handles the on reset request from the Twitch chat room
        /// </summary>
        /// <param name="twitchChat">Twitch Chat object</param>
        /// <param name="commandsArgs">The arguments recieved</param>
        public void OnReset(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            if(!commandsArgs.nick.Equals(m_Nick)) return;
            if(commandsArgs.IsCommand(m_ResetCommand))
            {
                TwitchChat_OnResetCommand(commandsArgs);
            }
        }

        /// <summary>
        /// Handles the on move request from the Twitch chat room
        /// </summary>
        /// <param name="twitchChat">Twitch Chat object</param>
        /// <param name="commandsArgs">The arguments received</param>
        public void OnMove(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            if(!commandsArgs.nick.Equals(m_Nick)) return;
            if(commandsArgs.IsCommand(m_MoveCommand))
            {
                TwitchChat_OnMoveCommand(twitchChat, commandsArgs);
            }
        }

        /// <summary>
        /// Handles the on move in one direction request from the Twitch chat room
        /// </summary>
        /// <param name="twitchChat">Twitch Chat object</param>
        /// <param name="commandsArgs">The arguments received</param>
        public void OnMoveUniDirection(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            if(!commandsArgs.nick.Equals(m_Nick)) return;
            if(commandsArgs.IsCommand(m_XCommand))
            {
                TwitchChat_OnXCommand(twitchChat, commandsArgs);
            }
            else if(commandsArgs.IsCommand(m_YCommand))
            {
                TwitchChat_OnYCommand(twitchChat, commandsArgs);
            }
            else if(commandsArgs.IsCommand(m_ZCommand))
            {
                TwitchChat_OnZCommand(twitchChat, commandsArgs);
            }
        }

        void TwitchChat_OnXCommand(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            float value;
            if(float.TryParse(commandsArgs.argument, out value))
            {
                AddForceFromCommand(float.Parse(commandsArgs.argument),
                    (val) => new Vector3(val, 0.0f, 0.0f),
                    (val) => Mathf.Clamp(val.x, -m_MaxSpeed.x, m_MaxSpeed.x));
            }
            else
            {
                twitchChat.SendChatMessageFormat("@{0} {1} requires 1 argument and it must be a number you used ({2})", commandsArgs.nick, commandsArgs.command, commandsArgs.argument);
            }
        }

        void TwitchChat_OnYCommand(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            float value;
            if(float.TryParse(commandsArgs.argument, out value))
            {
                AddForceFromCommand(float.Parse(commandsArgs.argument),
                    (val) => new Vector3(0.0f, val, 0.0f),
                    (val) => Mathf.Clamp(val.y, 0.0f, m_MaxSpeed.y));
            }
            else
            {
                twitchChat.SendChatMessageFormat("@{0} {1} requires 1 argument and it must be a number you used ({2})", commandsArgs.nick, commandsArgs.command, commandsArgs.argument);
            }
        }

        void TwitchChat_OnZCommand(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            float value;
            if(float.TryParse(commandsArgs.argument, out value))
            {
                AddForceFromCommand(float.Parse(commandsArgs.argument),
                    (val) => new Vector3(0.0f, 0.0f, val),
                    (val) => Mathf.Clamp(val.z, -m_MaxSpeed.z, m_MaxSpeed.z));
            }
            else
            {
                twitchChat.SendChatMessageFormat("@{0} {1} requires 1 argument and it must be a number you used ({2})", commandsArgs.nick, commandsArgs.command, commandsArgs.argument);
            }
        }

        void TwitchChat_OnMoveCommand(TwitchChat twitchChat, CommandsArgs commandsArgs)
        {
            int requiredArgsCount = 3;
            if(commandsArgs.nArgument.Length == requiredArgsCount)
            {
                float rmx;
                float rmy;
                float rmz;

                float.TryParse(commandsArgs.nArgument[MX], out rmx);
                float.TryParse(commandsArgs.nArgument[MY], out rmy);
                float.TryParse(commandsArgs.nArgument[MZ], out rmz);

                MovePlayer(rmx, rmy, rmz);
            }
            else
            {
                twitchChat.SendChatMessageFormat("@{0} {1} requires {2} arguments", commandsArgs.nick, commandsArgs.command, requiredArgsCount);
            }
        }

        /// <summary>
        /// Sets movement vector to zero
        /// <para>Override in derived classes to add functionality</para>
        /// </summary>
        /// <param name="commandsArgs">Commands args received from the chat client</param>
        protected virtual void TwitchChat_OnResetCommand(CommandsArgs commandsArgs)
        {
            m_Movement = Vector3.zero;
        }

        void AddForceFromCommand(float value, Func<float, Vector3> toMovement, Func<Vector3, float> clamp)
        {
            m_RawMovement = toMovement.Invoke(value);
            float rawMovement = clamp.Invoke(m_RawMovement);
            m_Movement = toMovement.Invoke(rawMovement);
            
            OnMoveCommandReceived(m_Movement, m_RawMovement);
        }

        void MovePlayer(float rmx, float rmy, float rmz)
        {
            m_RawMovement = new Vector3(rmx, rmy, rmz);
            float mx = Mathf.Clamp(m_RawMovement.x, -m_MaxSpeed.x, m_MaxSpeed.x);
            float my = Mathf.Clamp(m_RawMovement.y, 0.0f, m_MaxSpeed.y);
            float mz = Mathf.Clamp(m_RawMovement.z, -m_MaxSpeed.z, m_MaxSpeed.z);
            m_Movement = new Vector3(mx, my, mz);
            
            OnMoveCommandReceived(m_Movement, m_RawMovement);
        }

        /// <summary>
        /// Sends the movement vectors to the derived classes
        /// </summary>
        /// <param name="movement">Clamped movement vector, movement clamped to max speed.</param>
        /// <param name="rawMovement">Unclamped movement vector</param>
        protected abstract void OnMoveCommandReceived(Vector3 movement, Vector3 rawMovement);

        /// <summary>
        /// <para>Override in derived classes to add functionality</para>
        /// </summary>
        /// <param name="nick">The nick for the player controller</param>
        /// <param name="startPosition">Required start position</param>
        public virtual void StartNew(string nick, Vector3 startPosition)
        {
            m_StartPosition = startPosition;
            this.nick = nick;
            this.displayName = nick;
        }
    }
}
