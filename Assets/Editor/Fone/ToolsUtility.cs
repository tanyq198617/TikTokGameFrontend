using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class ToolsUtility
{
    public static string GetImageIdInAtlas(Sprite sprite)
    {
        if (sprite == null)
            return null;

        SpriteAtlasUtility.PackAllAtlases(EditorUserBuildSettings.activeBuildTarget);

        string[] searchForders = { "Assets/HotUpdateResources/SpriteAtlas" };
        string[] atlasPaths = AssetDatabase.FindAssets("t:spriteatlas", searchForders);

        for (int i = 0; i < atlasPaths.Length; ++i)
        {
            string atlasPath = AssetDatabase.GUIDToAssetPath(atlasPaths[i]);
            SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);

            if (atlas.CanBindTo(sprite))
            {
                return string.Format("{0}@{1}", sprite.name, atlas.name);
            }
        }

        return null;
    }

    /// <summary>
    /// 复制文件夹中的所有内容
    /// </summary>
    /// <param name="sourceDirPath">源文件夹目录</param>
    /// <param name="saveDirPath">指定文件夹目录</param>
    public static void CopyDirectoryFiles(string sourceDirPath, string saveDirPath)
    {
        if (Directory.Exists(saveDirPath))
            Directory.Delete(saveDirPath, true);
        Directory.CreateDirectory(saveDirPath);

        string[] files = Directory.GetFiles(sourceDirPath);
        foreach (string file in files)
        {
            string pFilePath = Path.Combine(saveDirPath, Path.GetFileName(file));
            if (File.Exists(pFilePath))
                continue;
            File.Copy(file, pFilePath, true);
        }

        string[] dirs = Directory.GetDirectories(sourceDirPath);
        foreach (string dir in dirs)
        {
            CopyDirectoryFiles(dir, Path.Combine(saveDirPath, Path.GetFileName(dir)));
        }
    }
}
