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

using UniTwitchIRC.TwitchInterface;
using UnityEditor;

namespace UniTwitchIRCEditor
{
    /// <summary>
    /// Editor to draw List to enter to greetings expected
    /// </summary>
    [CustomEditor(typeof(GreetingsBehaviour))]
    public class GreetingsBehaviourEditor : Editor
    {
        RepliesSetReorderableList m_RepliesSetReorderableList;

        void OnEnable()
        {
            m_RepliesSetReorderableList = new RepliesSetReorderableList(serializedObject, serializedObject.FindProperty("m_Greetings"), "Greetings: {0}=nick {1}=greeting");
        }

        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();
            serializedObject.Update();
            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script", "m_Greetings" });
            m_RepliesSetReorderableList.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
