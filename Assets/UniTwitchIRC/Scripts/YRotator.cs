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

using System.Collections;
using UnityEngine;

namespace UniTwitchIRC
{
    /// <summary>
    /// Rotates a target transform around the y-axis
    /// </summary>
    public class YRotator : MonoBehaviour
    {
        [SerializeField]
        Transform m_Target = null;

        [SerializeField]
        float m_Angle = 3.0f;

        IEnumerator Start()
        {
            while(enabled)
            {
                m_Target.Rotate(Vector3.up, m_Angle);
                yield return new WaitForFixedUpdate();
            }
        }

    }
}
