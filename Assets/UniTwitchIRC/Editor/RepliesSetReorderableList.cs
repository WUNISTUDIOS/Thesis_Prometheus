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

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UniTwitchIRCEditor
{
    /// <summary>
    /// Draws list of replies set in a neat list
    /// </summary>
    public class RepliesSetReorderableList : ReorderableList
    {
        /// <summary>
        /// COnstructor for list of replies set in a neat list
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="elements"></param>
        /// <param name="headerText"></param>
        public RepliesSetReorderableList(SerializedObject serializedObject, SerializedProperty elements, string headerText)
            : base(serializedObject, elements, true, true, true, true)
        {
            base.headerHeight = EditorGUIUtility.singleLineHeight + 2;
            base.elementHeight = EditorGUIUtility.singleLineHeight + 4;

            base.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, new GUIContent(headerText, headerText));
            base.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 1;
                rect.height = EditorGUIUtility.singleLineHeight + 1;
                Rect phraseRect = rect;
                phraseRect.width *= 0.2f;

                Rect responseRect = rect;
                responseRect.width *= 0.8f;
                responseRect.x = phraseRect.xMax;
                SerializedProperty prop = base.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(phraseRect, prop.FindPropertyRelative("m_Message"), GUIContent.none);
                EditorGUI.PropertyField(responseRect, prop.FindPropertyRelative("m_Description"), GUIContent.none);
            };
        }

        /// <summary>
        /// Drawer
        /// </summary>
        public void OnInspectorGUI()
        {
            base.DoLayoutList();
        }
    }
}
