using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StreamingAssetsEditor : Editor
{
    [MenuItem("日志/打开日志目录", false, 901)]
    public static void TryOpenPersistentDataPath()
    {
        Application.OpenURL(Application.persistentDataPath);
    }
}
