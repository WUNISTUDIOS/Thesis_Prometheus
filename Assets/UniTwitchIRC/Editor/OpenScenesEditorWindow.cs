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

using TryItCompatibilityEditor;
using TwitchUnityIRC;
using UnityEditor;

namespace UniTwitchIRCEditor
{
    /// <summary>
    /// Opens example scene picker
    /// </summary>
    [InitializeOnLoad]
    internal sealed class OpenScenesEditorWindow
    {
        static bool s_IsFullVersion = !(TwitchUnityIRCSettings.Instance.GetTryIt());
        public static void DrawOpenTryItButton()
        {
            ShowTryIt.DrawOpenTryItButton(!s_IsFullVersion);
        }

        static OpenScenesEditorWindow()
        {
            //Debug.Log("OpenScenesEditorWindow UpdateShowPrompt");
            ExampleScenesWindow.newsletterSignupUrl = "http://eepurl.com/bPDq3P";
            ExampleScenesWindow.isFullVersion = s_IsFullVersion;
            ExampleScenesWindow.packageName = "Twitch API Integration";
            ExampleScenesWindow.buttonTexts = new SceneButton[]
            {
                new SceneButton("Open Haydens Arena Scene", "Haydens Arena", "Assets/UniTwitchIRC/Scenes/Haydens Arena.unity",
                    "This scene, Haydens Arena, is a quick show of how the main components are used.  Enter in your API client id and your IRC oauth token then press play to test the game connections.  The commands can be found on the Messages Filter component a chile of the prefab in the scene 'UniTwitch IRC Basic Example'."),
            };

            ExampleScenesWindow.fullVersionUrl = "http://u3d.as/nWC";
        }
        
        [MenuItem("Window/Twitch API Integration/Example Scenes")]
        internal static void Init()
        {
            ExampleScenesWindow.Init(height: 280, width: 280);
        }
    }
}
