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
using UniTwitchIRC.TwitchInterface;
using UnityEngine;

namespace UniTwitchIRC.Controllers
{
    /// <summary>
    /// Spawn pickups for the playes to aquire during game play
    /// </summary>
    public class PickUpSpawner : MonoBehaviour
    {

        [SerializeField]
        GameObject m_PrefabPickup = null;

        [SerializeField]
        float m_Radius = 10.0f;

        [SerializeField]
        float m_Height = 0.5f;

        [SerializeField]
        int m_MaxPickups = 5;

        [SerializeField]
        float m_Rate = 3.0f;

        [SerializeField]
        Color m_GizmoColor = Color.yellow;

        int m_CurrentCount = 0;

        void OnEnable()
        {
            PlayerPickupController.OnPickUp -= PlayerPickupController_OnPickUp;
            PlayerPickupController.OnPickUp += PlayerPickupController_OnPickUp;
        }

        void OnDisable()
        {
            PlayerPickupController.OnPickUp -= PlayerPickupController_OnPickUp;
        }

        void PlayerPickupController_OnPickUp(TwitchPlayerController playerController, GameObject obj)
        {
            m_CurrentCount--;
            Destroy(obj);
        }

        IEnumerator Start()
        {
            Transform trans = transform;
            while(enabled)
            {
                if(m_CurrentCount < m_MaxPickups)
                {
                    GameObject pickUpObj = GameObject.Instantiate<GameObject>(m_PrefabPickup);
                    Vector3 area = Random.insideUnitSphere * m_Radius;
                    area.y = m_Height;

                    pickUpObj.transform.position = area;
                    pickUpObj.name = m_PrefabPickup.name;
                    pickUpObj.transform.SetParent(trans);
                    m_CurrentCount++;
                }
                yield return new WaitForSeconds(m_Rate);
            }
        }

        void OnDrawGizmosSelected()
        {
            Color color = Gizmos.color;
            Gizmos.color = m_GizmoColor;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
            Gizmos.color = color;
        }
    }
}
