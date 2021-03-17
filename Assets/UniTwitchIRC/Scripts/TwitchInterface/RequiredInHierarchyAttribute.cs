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
    /// Attribute to specify that the required type is present in the hierarchy and scene view
    /// </summary>
    [System.Serializable]
    public class RequiredInHierarchyAttribute : PropertyAttribute
    {
        /// <summary>
        /// The type of component required
        /// </summary>
        public readonly Type requiredType;

        /// <summary>
        /// Constructor
        /// <para>To specify that the required type is present in the hierarchy and scene view</para>
        /// </summary>
        /// <param name="requiredType">The type of component required</param>
        public RequiredInHierarchyAttribute(Type requiredType)
        {
            this.requiredType = requiredType;
        }
    }
}