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

using System.Diagnostics;
using System.IO;
using System.Linq;
using TwitchUnityIRC;
using UniTwitchIRC.TwitchInterface;
using UniTwitchIRC.TwitchInterface.PollManagement;
using UniTwitchIRCEditor.PollManagement;
using UnityEditor;
using UnityEngine;

namespace UniTwitchIRCEditor
{
    /// <summary>
    /// UniTwitch IRC drawer for combined basic components
    /// </summary>
    [CustomEditor(typeof(UniTwitchIRCBehaviours))]
    public class UniTwitchIRCBehavioursEditor : Editor
    {
        const string BOX_STYLE_NAME = "Box";
        const string OPEN_CHAT_POPUP_BUTTON_LABEL = "Open UniTwitch ChatPopup";
        const string DOWNLOAD_CHAT_POPUP_BUTTON_LABEL = "Download UniTwitch ChatPopup";

        static GUIContent s_ViewAboutChatButtonLabel = new GUIContent("?", "Read About UniTwitch Chat Popup");
        static GUIContent s_OpenOnRunLabel = new GUIContent("On Run", "Open the chat popu on playmode start.");
        
        static string s_ChatPopupURL = "http://callowcreation.com/products/unity/unitwitch-irc/";
        static string s_ChatPopupInformationURL = "http://callowcreation.com/home/unitwitch-irc-chat-popup/";
        static string s_ChatPopupName = Path.GetFileName("UniTwitchChatPopup");
        static string s_ChatPopupFileName = Path.GetFileName(s_ChatPopupName + ".exe");
        static string s_ChatPopupZipFileName = Path.GetFileName(s_ChatPopupName + ".zip");
        static string s_ChatPopupUrlZipFileName = s_ChatPopupURL + s_ChatPopupName + ".zip";
        static Process[] m_ChatPopupProcesses;

        UniTwitchIRCBehaviours m_UniTwitchIRCBehaviours = null;
        TwitchChat m_TwitchChat = null;
        TwitchChatEditor m_TwitchChatEditor = null;
        TwitchConnect m_TwitchConnect = null;
        TwitchConnectEditor m_TwitchConnectEditor = null;
        PollManager m_PollManager = null;
        PollManagerEditor m_PollManagerEditor = null;

        SerializedProperty m_ChannelProperty = null;
        [System.Obsolete("Discontiued the chat popup support", true)]
        SerializedProperty m_OpenOnRunProperty = null;

        bool m_Downloading = false;
        bool m_StartingProcess = false;

        void OnEnable()
        {
            m_UniTwitchIRCBehaviours = target as UniTwitchIRCBehaviours;

            CreateEditorsIfNeeded();
        }

        void CreateEditorsIfNeeded()
        {
            m_TwitchChat = m_UniTwitchIRCBehaviours.GetComponentInChildren<TwitchChat>();
            if (m_TwitchChat)
            {
                if (m_TwitchChatEditor == null)
                {
                    m_TwitchChatEditor = Editor.CreateEditor(m_TwitchChat, typeof(TwitchChatEditor)) as TwitchChatEditor;
                }
                m_ChannelProperty = m_TwitchChatEditor.serializedObject.FindProperty("m_Channel");
            }

            m_TwitchConnect = m_UniTwitchIRCBehaviours.GetComponentInChildren<TwitchConnect>();
            if (m_TwitchConnect)
            {
                if (m_TwitchConnectEditor == null)
                {
                    m_TwitchConnectEditor = Editor.CreateEditor(m_TwitchConnect) as TwitchConnectEditor;
                }
            }

            CreateEditor(ref m_TwitchChatEditor, m_TwitchChat);
            CreateEditor(ref m_TwitchConnectEditor, m_TwitchConnect);
            CreateEditor(ref m_PollManagerEditor, m_PollManager);
        }

        void CreateEditor<T, V>(ref T editor, V component) where T : Editor where V : MonoBehaviour
        {
            component = m_UniTwitchIRCBehaviours.GetComponentInChildren<V>();
            if (component)
            {
                if (editor == null)
                {
                    editor = Editor.CreateEditor(component) as T;
                }
            }
        }

        void DestroyEditorsIfNeeded()
        {
            DestroyEditor(ref m_TwitchChatEditor);
            DestroyEditor(ref m_TwitchConnectEditor);
            DestroyEditor(ref m_PollManagerEditor);
        }

        void OnDisable()
        {
            //UnSubscribeFromUpdate();
            DestroyEditorsIfNeeded();
        }

        void OnDestroy()
        {
            //UnSubscribeFromUpdate();
            DestroyEditorsIfNeeded();
        }

        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();

            DrawEditorInspector(m_TwitchChatEditor);
            DrawEditorInspector(m_TwitchConnectEditor);
            DrawEditorInspector(m_PollManagerEditor);

            serializedObject.ApplyModifiedProperties();
        }

        static void DestroyEditor<T>(ref T editor) where T : Editor
        {
            if (editor != null)
            {
                DestroyImmediate(editor);
                editor = null;
            }
        }

        static void DrawEditorInspector<T>(T editor) where T : Editor
        {
            if (editor)
            {
                GUILayout.BeginVertical(BOX_STYLE_NAME);
                {
                    GUIStyle style = new GUIStyle(EditorStyles.largeLabel);
                    style.fontStyle = FontStyle.Bold;
                    style.alignment = TextAnchor.MiddleCenter;
                    EditorGUILayout.LabelField(editor.target.name, style);
                    editor.OnInspectorGUI();
                }
                GUILayout.EndVertical();
            }
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void OnUpdateEditor()
        {
            MonitorChatPopupProcess();
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void UnSubscribeFromUpdate()
        {
            EditorApplication.update -= OnUpdateEditor;
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void SubscribeToUpdate()
        {
            EditorApplication.update -= OnUpdateEditor;
            EditorApplication.update += OnUpdateEditor;
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void MonitorChatPopupProcess()
        {
#if UNITY_EDITOR_WIN
            m_ChatPopupProcesses = Process.GetProcessesByName(s_ChatPopupName);
            if(m_ChatPopupProcesses.Length == 0)
            {
                Repaint();
            }
#endif
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void ShowOpenProcessButton()
        {
#if UNITY_EDITOR_WIN
            if(m_ChatPopupProcesses != null && m_ChatPopupProcesses.Length == 0 && !m_StartingProcess)
            {
                if(File.Exists(s_ChatPopupFileName) && !File.Exists(s_ChatPopupZipFileName))
                {
                    if(!string.IsNullOrEmpty(m_ChannelProperty.stringValue))
                    {
                        GUILayout.BeginHorizontal(BOX_STYLE_NAME);
                        {
                            if(GUILayout.Button(OPEN_CHAT_POPUP_BUTTON_LABEL))
                            {
                                StartChatPopupProcess();
                            }
                            m_OpenOnRunProperty.boolValue = EditorGUILayout.ToggleLeft(s_OpenOnRunLabel, m_OpenOnRunProperty.boolValue);
                            if(m_OpenOnRunProperty.boolValue && EditorApplication.isPlayingOrWillChangePlaymode)
                            {
                                if(EditorApplication.isPlaying)
                                {
                                    StartChatPopupProcess();
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                else if(!m_Downloading)
                {
                    GUILayout.BeginHorizontal(BOX_STYLE_NAME);
                    {
                        if(GUILayout.Button(DOWNLOAD_CHAT_POPUP_BUTTON_LABEL, GUI.skin.button))
                        {
                            m_Downloading = true;
                            ZipUtility.Compression.Download(s_ChatPopupUrlZipFileName, s_ChatPopupZipFileName);
                            ZipUtility.Compression.UnZip(s_ChatPopupZipFileName);
                            File.Delete(s_ChatPopupZipFileName);
                            m_Downloading = false;
                        }
                        if(GUILayout.Button(s_ViewAboutChatButtonLabel, GUILayout.MaxWidth(20)))
                        {
                            Application.OpenURL(s_ChatPopupInformationURL);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
#endif
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void StartChatPopupProcess()
        {
            m_StartingProcess = true;
            Process.Start(s_ChatPopupFileName, m_ChannelProperty.stringValue);
        }    
    }
}
