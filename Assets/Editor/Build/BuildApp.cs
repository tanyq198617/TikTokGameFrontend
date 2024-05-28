using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildApp : MonoBehaviour
{
    private static string RootDir = "Build";
    private static string packageName = "wjxqc";
    private static string copyPath = "";


    [MenuItem("打包/BuildWinCheckSVN &B")]
    public static void BuildByCommandLineCheckSVN()
    {
        // 获取传入的参数
        // 读取传入命令行
        var args = System.Environment.GetCommandLineArgs();
        foreach (string arg in args)
        {
            Debug.Log(arg);
        }

        //静默更新
        SVNEditor.SVNUpdateCmd();
        ExecuteBuild();
    }

    [MenuItem("打包/BuildWin")] 
    public static void BuildByCommandLine()
    {
        // 获取传入的参数
        // 读取传入命令行
        var args = System.Environment.GetCommandLineArgs();
        foreach (string arg in args)
        {
            Debug.Log(arg);
        }

        ExecuteBuild();
    }

    private static void ExecuteBuild()
    {
        //初始化打包参数
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions(); ;
        buildPlayerOptions.scenes = GetBuildScenes();
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        string dir = $"{RootDir}/{packageName}";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        buildPlayerOptions.locationPathName = $"{dir}/{packageName}.exe";
        buildPlayerOptions.targetGroup = BuildTargetGroup.Standalone;
        buildPlayerOptions.options = BuildOptions.None;
        
        //打包
        Directory.Delete(dir, true);
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (!Directory.Exists(dir) || Directory.GetFiles(dir).Length < 1)
        {
            Debug.LogError("打包失败了...");
            return;
        }

        DeleteDevelopDir(dir);

        //压缩
        // TextAsset text = Resources.Load("config") as TextAsset;
        // LitJson.JsonData data = LitJson.JsonMapper.ToObject(text.text);
        // var _version = data["AppVersion"].ToString();
        
        // var zipPath = $"{RootDir}/{packageName}-{_version}_{TimeToName()}.zip";
        // if(File.Exists(zipPath)) File.Delete(zipPath);
        // ZipUtility.Zip(new []{ dir }, zipPath);

        //拷贝
        // if (!string.IsNullOrEmpty(copyPath))
        //     File.Copy(zipPath, $"{copyPath}/{packageName}_{TimeToName()}.zip");
        
        Debug.Log("打包完成！");  
        Application.OpenURL(RootDir);
    }

    private static string TimeToName()
    {
        DateTime now = DateTime.Now;
        int minutesPassed = now.Hour * 60 + now.Minute;
        return $"{now:yyyyMMdd}_{minutesPassed}";
    }

    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }

    private static void DeleteDevelopDir(string root)
    {
        if (!Directory.Exists(root))
            Directory.CreateDirectory(root);
        var dirs = Directory.GetDirectories(root);
        for (int i = 0; i < dirs.Length; i++)
        {
            if (dirs[i].EndsWith("_BackUpThisFolder_ButDontShipItWithYourGame") ||
                dirs[i].EndsWith("_BurstDebugInformation_DoNotShip"))
            {
                Directory.Delete(dirs[i], true);
            }
        }
    }
}
