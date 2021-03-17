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
using UniTwitchIRC.Controllers;
using UniTwitchIRC.TwitchInterface;
using UnityEngine;
using UnityEngine.UI;

namespace UniTwitchIRC.ExampleUI
{
    /// <summary>
    /// The player UI in the world shows display name and points
    /// </summary>
    public class PlayerUI : MonoBehaviour, IPlayerUI
    {

        [SerializeField]
        TwitchPlayerController m_PlayerController = null;

        [SerializeField]
        Text m_TextNick = null;
        
        [SerializeField]
        Text m_TextPoints = null;

        [SerializeField]
        Vector3 m_Offset = Vector3.up * 2.0f;

        [SerializeField]
        float m_SmoothTime = 1.0f;

        /// <summary>
        /// Access to the UI player controller
        /// </summary>
        public TwitchPlayerController playerController { set { m_PlayerController = value; } }

        Transform m_Target = null;
        Transform m_Trans = null;
        Vector3 m_CurrentVelocity = Vector3.zero;

        IEnumerator Start()
        {
            while(!m_PlayerController)
            {
                yield return new WaitForEndOfFrame();
            }
            m_Trans = transform;
            m_Target = m_PlayerController.transform;
            string format = m_TextPoints.text;

            while(enabled)
            {
                m_TextNick.text = m_PlayerController.displayName;
                m_TextPoints.text = string.Format(format, m_PlayerController.points);
                m_Trans.position = Vector3.SmoothDamp(m_Trans.position, m_Target.position, ref m_CurrentVelocity, m_SmoothTime);
                m_Trans.position += m_Offset;
                yield return new WaitForFixedUpdate();
            }
        }

    }
}
