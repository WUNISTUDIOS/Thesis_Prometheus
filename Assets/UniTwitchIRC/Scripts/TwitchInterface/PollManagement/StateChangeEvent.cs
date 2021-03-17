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
using System.Collections.Generic;

namespace UniTwitchIRC.TwitchInterface.PollManagement
{
    public class StateChangeEventArgs
    {
        private List<Option> options;
        private PollState state;

        public StateChangeEventArgs(List<Option> options, PollState state)
        {
            this.options = options;
            this.state = state;
        }
    }

    [Serializable]
    internal class StateChangeEvent : UnityEngine.Events.UnityEvent<StateChangeEventArgs> { }
}