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
namespace UniTwitchIRC
{
    /// <summary>
    /// Extension to validate that commandsArgs command and strings are indeed commands
    /// </summary>
    public static class HelperExtensions
    {
        /// <summary>
        /// Expected command symbol usually a ! for Twitch chat
        /// </summary>
        public const string COMMAND_SYMBOL = "!";

        /// <summary>
        /// Validates that a CommandsArgs is the command you are interested in using
        /// </summary>
        /// <param name="commandsArgs">CommandsArgs received from the client</param>
        /// <param name="command">Command you are interested in using</param>
        /// <returns>True if the parameters are a match, False otherwise</returns>
        public static bool IsCommand(this CommandsArgs commandsArgs, string command)
        {
            return commandsArgs.command.Equals(string.Concat(COMMAND_SYMBOL, command));
        }

        /// <summary>
        /// Validates that a string is the command you are interested in using
        /// </summary>
        /// <param name="instance">string received from the client as a command message</param>
        /// <param name="command">Command you are interested in using</param>
        /// <returns>True if the parameters are a match, False otherwise</returns>
        public static bool IsCommand(this string instance, string command)
        {
            return instance.Equals(string.Concat(COMMAND_SYMBOL, command));
        }
    }
}