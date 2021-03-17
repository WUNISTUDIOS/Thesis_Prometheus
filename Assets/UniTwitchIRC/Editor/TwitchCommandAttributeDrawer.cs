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

using System.Collections.Generic;
using System.Linq;
using UniTwitchIRC.TwitchInterface;
using UnityEditor;
using UnityEngine;

namespace UniTwitchIRCEditor
{
    /// <summary>
    /// Drawer for popup to select a Twitch command for the component to use
    /// </summary>
    [CustomPropertyDrawer(typeof(TwitchCommandAttribute))]
    public class TwitchCommandAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        /// Draws popup in the Inspector
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
        {
            CommandsBehaviour commandsBehaviour = GameObject.FindObjectOfType<CommandsBehaviour>();
            if(commandsBehaviour == null)
            {
                EditorGUI.HelpBox(position, "Can not find a CommandsBehaviour component in the scene.", MessageType.Warning);
                return;
            }
            List<RepliesSet> repliesSets = commandsBehaviour.GetRepliesSets().ToList();
            string[] displayOptions = repliesSets.Select(x => x.message).ToArray();
            int index = repliesSets.FindIndex(x => x.message == property.stringValue);
            index = EditorGUI.Popup(position, label.text, index, displayOptions);
            if(index > -1)
            {
                property.stringValue = displayOptions[index];
            }
        }

        /// <summary>
        /// Gets the height needed for drawing the property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CommandsBehaviour commandsBehaviour = GameObject.FindObjectOfType<CommandsBehaviour>();
            if(commandsBehaviour == null)
            {
                return base.GetPropertyHeight(property, label) * 2.0f;
            }
            return base.GetPropertyHeight(property, label);
        }
    }
}
