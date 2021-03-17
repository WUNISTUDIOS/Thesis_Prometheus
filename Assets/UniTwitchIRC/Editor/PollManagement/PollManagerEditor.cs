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
    /// Drawer for Poll Manager component
    /// </summary>
    [CustomEditor(typeof(PollManager))]
    public class PollManagerEditor : Editor
    {
        const string LABEL_OPEN_POLL = "Open Poll";
        const string LABEL_QUESTION = "Poll Question";
        const string LABEL_OPTIONS = "Poll Options";
        const string LABEL_VALUE = "Option";
        const string LABEL_VOTES = "Votes";
        const string FIELD_OPEN_POLL = "m_OpenPoll";
        const string FIELD_QUESTION = "m_Question";
        const string FIELD_OPTIONS = "m_Options";
        const string FIELD_SCRIPT = "m_Script";
        const string PROPERTY_NAME_INDEX = "index";
        const string PROPERTY_NAME_VALUE = "value";
        const string PROPERTY_NAME_VOTES = "votes";

        static readonly string[] s_OmitFields = new string[] { FIELD_SCRIPT, FIELD_OPEN_POLL, FIELD_QUESTION, FIELD_OPTIONS };

        const float PADDING = 2.0f;

        SerializedProperty m_OpenPollProperty = null;

        SerializedProperty m_QuestionProperty = null;

        SerializedProperty m_OptionsProperty = null;

        ReorderableList m_OptionsReorderableList = null;

        static int s_PollManagerCount = 0;

        void OnEnable()
        {
            s_PollManagerCount = FindObjectsOfType<PollManager>().Length;
            if (s_PollManagerCount > 1)
            {
                return;
            }

            m_OpenPollProperty = serializedObject.FindProperty(FIELD_OPEN_POLL);
            m_QuestionProperty = serializedObject.FindProperty(FIELD_QUESTION);
            m_OptionsProperty = serializedObject.FindProperty(FIELD_OPTIONS);

            m_OptionsReorderableList = new ReorderableList(serializedObject, m_OptionsProperty, true, true, true, true);
            m_OptionsReorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, LABEL_OPTIONS);
            m_OptionsReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty property = m_OptionsReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                SerializedProperty indexProp = property.FindPropertyRelative(PROPERTY_NAME_INDEX);
                SerializedProperty valueProp = property.FindPropertyRelative(PROPERTY_NAME_VALUE);
                SerializedProperty votesProp = property.FindPropertyRelative(PROPERTY_NAME_VOTES);

                float width = rect.width;
                float valuePairWidth = width * 0.7f;
                float votesPairWidth = width * 0.3f - PADDING * 5;

                float valueLabelWidth = valuePairWidth * 0.2f;
                float valueWidth = valuePairWidth * 0.8f;

                float votesLabelWidth = votesPairWidth * 0.5f;
                float votesWidth = votesPairWidth * 0.5f;

                Rect valueLabelRect = new Rect(rect);
                valueLabelRect.width = valueLabelWidth;
                valueLabelRect.x = rect.x + PADDING;

                Rect valueRect = new Rect(rect);
                valueRect.width = valueWidth;
                valueRect.x = valueLabelRect.x + valueLabelRect.width + PADDING;

                Rect votesLabelRect = new Rect(rect);
                votesLabelRect.x = valueRect.x + valueRect.width + PADDING;
                votesLabelRect.width = votesLabelWidth;

                Rect votesRect = new Rect(rect);
                votesRect.x = votesLabelRect.x + votesLabelRect.width + PADDING;
                votesRect.width = votesWidth;

                EditorGUI.LabelField(valueLabelRect, LABEL_VALUE);
                votesProp.intValue = EditorGUI.IntField(votesRect, votesProp.intValue);
                EditorGUI.LabelField(votesLabelRect, LABEL_VOTES);
                valueProp.stringValue = EditorGUI.TextField(valueRect, valueProp.stringValue);
            };
        }

        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();

            if (s_PollManagerCount > 1)
            {
                EditorGUILayout.HelpBox("There sould only be one Poll Manager in the scene please remove one", MessageType.Error);
                return;
            }

            serializedObject.Update();

            m_OpenPollProperty.boolValue = EditorGUILayout.Toggle(LABEL_OPEN_POLL, m_OpenPollProperty.boolValue);

            m_QuestionProperty.stringValue = EditorGUILayout.TextField(LABEL_QUESTION, m_QuestionProperty.stringValue);

            Editor.DrawPropertiesExcluding(serializedObject, s_OmitFields);

            m_OptionsReorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
