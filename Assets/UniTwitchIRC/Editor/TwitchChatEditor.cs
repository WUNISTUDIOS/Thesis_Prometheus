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
using UnityEngine;

namespace UniTwitchIRCEditor
{
    /// <summary>
    /// Drawer for Twitch chat component
    /// </summary>
    [CustomEditor(typeof(TwitchChat))]
    public class TwitchChatEditor : Editor
    {
        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();
            serializedObject.Update();
            SerializedProperty hideRunInBackgroundMessageProp = serializedObject.FindProperty("m_HideRunInBackgroundMessage");
            if(!hideRunInBackgroundMessageProp.boolValue)
            {
                if(!Application.runInBackground)
                {
                    EditorGUILayout.HelpBox("Player run in background is not checked and will require the program to be showing to use the chat client.", MessageType.Warning);
                    EditorGUILayout.PropertyField(hideRunInBackgroundMessageProp, new GUIContent("Hide This Message"));
                    if(GUILayout.Button("Check It Now"))
                    {
                        Application.runInBackground = true;
                    }
                }
            }
            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script" });
            serializedObject.ApplyModifiedProperties();

            string nick = serializedObject.FindProperty("m_Nick").stringValue;
            string channel = serializedObject.FindProperty("m_Channel").stringValue;
            bool sameValues = nick.Trim().ToLower().Equals(channel.Trim().ToLower());
            if(!string.IsNullOrEmpty(nick) && !string.IsNullOrEmpty(channel) && sameValues)
            {
                EditorGUILayout.HelpBox("The nick and channel can not be the same value the component will not work properly.\nThis will cause a conflict and cause a massive slow-down in processing (lag) at run-time.", MessageType.Error);
            }
        }
    }
}
