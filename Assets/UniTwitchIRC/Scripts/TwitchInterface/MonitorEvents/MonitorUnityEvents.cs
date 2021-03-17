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

using IRCnect.Channel.Monitor;
using IRCnect.Channel.Monitor.Replies.Actions;
using IRCnect.Channel.Monitor.Replies.Inbounds;
using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using UnityEngine.Events;

namespace UniTwitchIRC.TwitchInterface.MonitorEvents
{

    #region MonitorAnyEvents

    /// <summary>
    /// Params: [<string>MonitorArgs</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorAnyEvent : UnityEvent<MonitorArgs> { }

    /// <summary>
    /// Params: [<string>raw data</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorRawData : UnityEvent<string> { }

    #endregion


    #region MonitorGreetingsEvents

    /// <summary>
    /// Params: [<string>InboundsArgs</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorAnyInbounds : UnityEvent<InboundsArgs> { }

    /// <summary>
    /// Params: [<string>greeting</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorGreet : MonitorRawData { }

    /// <summary>
    /// Params: [<string>nick</string>, <string>said</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorNickSaid : MonitorCommand { }

    #endregion

    #region MonitorWhisperEvents

    /// <summary>
    /// Params: [<string>WhisperArgs</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorAnyWhispers : UnityEvent<ActionsArgs> { }

    /// <summary>
    /// Params: [<string>who</string> <string>message</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorMessage : UnityEvent<string, string> { }

    #endregion

    #region MonitorCommandsEvents

    /// <summary>
    /// Params: [<string>CommandsArgs</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorAnyCommands : UnityEvent<CommandsArgs> { }

    /// <summary>
    /// Params: [<string>nick</string>, <string>command</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorCommand : UnityEvent<string, string> { }

    /// <summary>
    /// Params: [<string>nick</string>, <string>command</string>, <string>argument</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorParam : UnityEvent<string, string, string> { }

    /// <summary>
    /// Params: [<string>nick</string>, <string>command</string>, <string>nArgument</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorNArgunment : UnityEvent<string, string, string[]> { }

    #endregion


    #region MonitorNumberEvents

    /// <summary>
    /// Params: [<string>nick</string>, <string>command</string>, <string>argument</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorInt : UnityEvent<string, string, int> { }

    /// <summary>
    /// Params: [<string>nick</string>, <string>command</string>, <string>argument</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorFloat : UnityEvent<string, string, float> { }

    /// <summary>
    /// Params: [<string>nick</string>, <string>command</string>, <string>nArgument</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorIntArray : UnityEvent<string, string, int[]> { }

    /// <summary>
    /// Params: [<string>nick</string>, <string>command</string>, <string>nArgument</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorFloatArray : UnityEvent<string, string, float[]> { }

    /// <summary>
    /// Params: [<string>TwitchChat</string>, <string>CommandsArgs</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorTwitchCommand : UnityEvent<TwitchChat, CommandsArgs> { }

    #endregion


    #region MonitorStringsEvents

    /// <summary>
    /// Params: [<string>nick</string>, <string>command</string>, <string>argument</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorString : MonitorParam { }

    /// <summary>
    /// Params: [<string>nick</string>, <string>command</string>, <string>nArgument</string>]
    /// </summary>
    [System.Serializable]
    public class MonitorStringArray : MonitorNArgunment { }

    #endregion
}