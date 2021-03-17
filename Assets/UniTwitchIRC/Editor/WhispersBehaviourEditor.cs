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
    /// Editor to draw List to enter the whisper component
    /// </summary>
    [CustomEditor(typeof(WhispersBehaviour))]
    public class WhispersBehaviourEditor : Editor
    {
        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();
            DrawDefaultInspector();
        }

    }
}
