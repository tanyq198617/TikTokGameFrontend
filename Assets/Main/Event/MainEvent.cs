using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此脚本仅供主工程使用，热更工程禁止写入代码
/// </summary>
internal class MainEvent
{
    public const string App_Version = "AppVersion";
    public const string Assets_Progress_Start = "AssetsProgressStart";
    public const string Assets_Progress_End = "AssetsProgressEnd";
    public const string Assets_Progress_Update = "AssetsProgressUpdate";
    public const string Assets_Download_Failed = "AssetsDownloadFailed";
    public const string Boot_Message = "BootMessage";

    public const string Version_NewMessage = "VersionNewMessage";
}
