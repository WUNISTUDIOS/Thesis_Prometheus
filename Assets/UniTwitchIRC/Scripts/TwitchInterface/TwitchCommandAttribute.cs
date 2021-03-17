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

using System;
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Attribute to specify that the string field will be used to reference a command
    /// <para>This will draw a popup for selecting the available commands</para>
    /// </summary>
    [Serializable]
    public class TwitchCommandAttribute : PropertyAttribute { }
}