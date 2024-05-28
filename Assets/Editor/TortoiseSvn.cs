using System.Diagnostics;
using UnityEngine;
using UnityEditor;
 
public class TortoiseSvn
{ 
    public static void SVNCommand(string command, string path)
    {
        //UnityEngine.Debug.LogError("command:"+command+"    path:"+path);
        string c = "/c tortoiseproc.exe /command:{0} /path:\"{1}\"";
        c = string.Format(c, command, path);
        ProcessStartInfo info = new ProcessStartInfo("cmd.exe", c);
        info.WindowStyle = ProcessWindowStyle.Hidden;
        Process.Start(info);
    }

    [MenuItem("Assets/SVN/提交程序,美术svn")]// %&c
    static void CommitRoot()
    {
        SVNCommand("commit", Application.dataPath);
        SVNCommand("commit", Application.dataPath + "/ArtAssets");
    }

    [MenuItem("Assets/SVN/Commit...")]
    static void CommitSelection()
    {
        SVNCommand("commit", GetPathFromSelectionWithMetas());
    }

    [MenuItem("Assets/SVN/Commit...", true)]
    static bool CommitSelectionValidation()
    {
        return IsAnythingSelected();
    }

    [MenuItem("Assets/SVN/Repo Browser")]
    static void ShowRepoBrowser()
    {
        SVNCommand("repobrowser", Application.dataPath);
    }

    [MenuItem("Assets/SVN/更新程序,美术svn")]// %&u
    static void UpdateAll()
    {
        SVNCommand("update", Application.dataPath);
        SVNCommand("update", Application.dataPath + "/ArtAssets");
    }

    [MenuItem("Assets/SVN/Update...")]
    static void UpdateSelection()
    {
        SVNCommand("update", GetPathFromSelectionWithMetas());
    }

    [MenuItem("Assets/SVN/Update...", true)]
    static bool UpdateSelectionValidation()
    {
        return IsAnythingSelected();
    }

    [MenuItem("Assets/SVN/Add All...")]// %&d
    static void AddAll()
    {
        SVNCommand("add", Application.dataPath);
    }

    [MenuItem("Assets/SVN/Add...")]
    static void AddSelection()
    {
        SVNCommand("add", GetPathFromSelectionWithMetas());
    }

    [MenuItem("Assets/SVN/Add...", true)]
    static bool AddSelectionValidation()
    {
        return IsAnythingSelected();
    }

    [MenuItem("Assets/SVN/Delete...")]
    static void DeleteSelection()
    {
        SVNCommand("remove", GetPathFromSelectionWithMetas());
    }

    [MenuItem("Assets/SVN/Delete...", true)]
    static bool DeleteSelectionValidation()
    {
        return IsAnythingSelected();
    }

    [MenuItem("Assets/SVN/CleanUp")]
    static void CleanUpFromSvn()
    {
        SVNCommand("cleanup", Application.dataPath);
    }

    static string GetPathFromSelection()
    {
        string p = "";
        foreach (UnityEngine.Object o in Selection.objects)
        {
            if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o)) == false)
            {
                if (p != "")
                    p += "*";
                p += Application.dataPath + "/../" + AssetDatabase.GetAssetPath(o);
            }
        }
        return p;
    } 
     
    static string GetPathFromSelectionWithMetas()
    {
        string p = "";
        foreach (UnityEngine.Object o in Selection.objects)
        {
            if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(o)) == false)
            {
                if (p != "")
                    p += "*"; 
                p += Application.dataPath.Replace("Assets",string.Empty)+ AssetDatabase.GetAssetPath(o);
                p += "*" + Application.dataPath.Replace("Assets", string.Empty) + AssetDatabase.GetAssetPath(o) + ".meta";
            }
        }
        p = p.Trim();
        return p;
    }
     
    static bool IsAnythingSelected()
    {
        if (GetPathFromSelection() != "")
            return true;
        return false;
    }
}