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

using IRCnect.Channel.Monitor.Replies.Inbounds;
using System;

namespace UniTwitchIRC.TwitchInterface.PollManagement
{
    public class VoteChangeEventArgs
    {
        internal Option option;
        public InboundsArgs inboundsArgs;

        public VoteChangeEventArgs(Option option, InboundsArgs inboundsArgs)
        {
            this.option = option;
            this.inboundsArgs = inboundsArgs;
        }
    }

    [Serializable]
    internal class VoteChangeEvent : UnityEngine.Events.UnityEvent<VoteChangeEventArgs> { }
}