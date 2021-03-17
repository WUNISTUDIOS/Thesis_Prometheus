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

namespace UniTwitchIRC.TwitchInterface.PollManagement
{
    [System.Serializable]
    public struct Option
    {
        public int index;
        public string value;
        public int votes;
    }
}