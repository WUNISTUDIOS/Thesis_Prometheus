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
    /// Component drawer for required items
    /// </summary>
    [CustomPropertyDrawer(typeof(RequiredInHierarchyAttribute))]
    public class RequiredInHierarchyAttributeDrawer : PropertyDrawer
    {
        static Object FindRequiredComponent(RequiredInHierarchyAttribute requiredAttr)
        {
            return GameObject.FindObjectOfType(requiredAttr.requiredType);
        }

        /// <summary>
        /// Draws popup in the Inspector
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RequiredInHierarchyAttribute requiredAttr = attribute as RequiredInHierarchyAttribute;
            if(!FindRequiredComponent(requiredAttr))
            {
                position.height = EditorGUIUtility.singleLineHeight * 2.0f;
                EditorGUI.HelpBox(position, string.Format("Can not find a {0} component in the scene.", requiredAttr.requiredType.Name), MessageType.Warning);
                position.y += EditorGUIUtility.singleLineHeight * 2.0f + 2;
                position.height = EditorGUIUtility.singleLineHeight;
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

        /// <summary>
        /// Gets the height needed for drawing the property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(!FindRequiredComponent(attribute as RequiredInHierarchyAttribute))
            {
                return EditorGUIUtility.singleLineHeight * 3.0f + 2.0f;
            }
            return base.GetPropertyHeight(property, label);
        }
    }
}
