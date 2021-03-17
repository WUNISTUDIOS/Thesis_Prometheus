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

using UniTwitchIRC.TwitchInterface.PollManagement;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UniTwitchIRCEditor.PollManagement
{
    /// <summary>
    /// Drawer for Poll Events component
    /// </summary>
    [CustomEditor(typeof(PollEvents))]
    public class PollEventsEditor : Editor
    {
        static int s_PollEventsCount = 0;

        void OnEnable()
        {
            s_PollEventsCount = FindObjectsOfType<PollEvents>().Length;
            if (s_PollEventsCount > 1)
            {
                return;
            }
        }

        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();

            if (s_PollEventsCount > 1)
            {
                EditorGUILayout.HelpBox("There sould only be one Poll Events in the scene please remove one", MessageType.Error);
                return;
            }

            Editor.DrawPropertiesExcluding(serializedObject, new [] { "m_Script" });
        }
    }
}
