using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using Debug = UnityEngine.Debug;

public class SVNEditor : Editor
{
    //E:\WorkSpace\DanMu_kuaishou\DanMuGame
    //更新指令 svn update --accept theirs-full {path} --username {user} --password {password}
    //更新使用他人版本 svn update --accept theirs-full {path} --username {user} --password {password}
    //清理指令 svn cleanup {clean_path} --username {user} --password {password}
    //解决冲突用他人版本 svn resolve -R --accept theirs-full {path} --username {user} --password {password}
    //还原指令 svn revert -R -q {path} --username {user} --password {password}
    //提交指令 svn commit {file_path} -m \"{commit_message}\"
    //获取差异 svn diff -r {l_version}:{r_version} --summarize {path} --username {user} --password {password}
    //获取状态 svn status {file_path} --username {user} --password {password}
    
    [MenuItem("SVN/更新(静默)", false, 2)]
    public static void SVNUpdateCmd()
    {
        ProcessCommand("svn", $"update --accept theirs-full {SVNProjectPath}");
    }
    
    [MenuItem("SVN/更新主工程", false, 2)]
    public static void SVNUpdate()
    {
        ProcessCommand ("TortoiseProc.exe", "/command:update /path:" + SVNProjectPath + " /closeonend:0");
    }
    
    [MenuItem("SVN/更新美术库", false, 2)]
    public static void SVNUpdateArt()
    {
        ProcessCommand ("TortoiseProc.exe", "/command:update /path:" + SVNArtPath + " /closeonend:0");
    }

    [MenuItem("SVN/提交", false, 1)]
    public static void SVNCommit()
    {
        List<string> pathList = new List<string>();
        string basePath = SVNProjectPath + "/Assets";
        pathList.Add(basePath);
        pathList.Add(SVNProjectPath + "/ProjectSettings");
        pathList.Add(SVNProjectPath + "/Packages");

        string commitPath = string.Join("*", pathList.ToArray());
        //EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        ProcessCommand("TortoiseProc.exe", "/command:commit /path:" + commitPath + "  -m '定时自动提交'");
    }

    [MenuItem("SVN/还原", false, 3)]
    public static void Breaker()
    {
        ProcessCommand("TortoiseProc.exe", "/command:revert /path:" + SVNProjectPath);
    }

    [MenuItem("SVN/清理", false, 4)]
    public static void SVNCleanUp()
    {
        ProcessCommand("TortoiseProc.exe", "/command:cleanup /path:" + SVNProjectPath);
    }

    [MenuItem("SVN/日志", false, 5)]
    static void SVNLog()
    {
        ProcessCommand("TortoiseProc.exe", "/command:log /path:" + SVNProjectPath);
    }

    static string SVNProjectPath
    {
        get
        {
            System.IO.DirectoryInfo parent = System.IO.Directory.GetParent(Application.dataPath);
            return parent.ToString();
        }
    }
    
    static string SVNArtPath
    {
        get
        {
            return $"{Application.dataPath}/ArtAssets"; 
        }
    }

    public static void ProcessCommand(string command, string argument)
    {
        try
        {
            // Debug.Log($"{command} {argument}");
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(command);
            info.Arguments = argument;
            info.CreateNoWindow = false;
            info.ErrorDialog = true;
            info.UseShellExecute = true;
            // info.WindowStyle = ProcessWindowStyle.Hidden; //隐藏窗口

            if (info.UseShellExecute)
            {
                info.RedirectStandardOutput = false;
                info.RedirectStandardError = false;
                info.RedirectStandardInput = false;
            }
            else
            {
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.RedirectStandardInput = true;
                info.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
                info.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
            }

            System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

            if (!info.UseShellExecute)
            {
                Debug.Log(process.StandardOutput);
                Debug.Log(process.StandardError);
            }

            process.WaitForExit();
            process.Close();
        }
        catch (Exception e)
        {
            Debug.LogError($"无法使用CMD指令：{e.ToString()}");
        }
    }
}