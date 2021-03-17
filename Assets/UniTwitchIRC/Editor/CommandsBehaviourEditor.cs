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
    /// Editor to draw List to enter the commands expected
    /// </summary>
    [CustomEditor(typeof(CommandsBehaviour))]
    public class CommandsBehaviourEditor : Editor
    {
        RepliesSetReorderableList m_BasicCommandsReorderableList;
        RepliesSetReorderableList m_ParameterizedCommandsReorderableList;
        RepliesSetReorderableList m_NArgunmentCommandsReorderableList;

        void OnEnable()
        {
            m_BasicCommandsReorderableList = new RepliesSetReorderableList(
                    serializedObject,
                    serializedObject.FindProperty("m_BasicCommands"),
                    "Basic: Single command no parameters"
                    );
            m_ParameterizedCommandsReorderableList = new RepliesSetReorderableList(
                    serializedObject,
                    serializedObject.FindProperty("m_ParameterizedCommands"),
                    "Parameterized: Must use one parameter"
                    );
            m_NArgunmentCommandsReorderableList = new RepliesSetReorderableList(
                    serializedObject,
                    serializedObject.FindProperty("m_NArgunmentCommands"),
                    "N Argunment: One or more parameters"
                    );
        }

        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();
            serializedObject.Update();

            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script", "m_BasicCommands", "m_ParameterizedCommands", "m_NArgunmentCommands" });
            m_BasicCommandsReorderableList.OnInspectorGUI();
            m_ParameterizedCommandsReorderableList.OnInspectorGUI();
            m_NArgunmentCommandsReorderableList.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }

    }
}
