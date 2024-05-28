using System.IO;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;

public class EditorModeSimulateClear
{
    [MenuItem("YooAsset/编辑器模式缓存清理", false, 403)]
    public static void SimulateClear()
    {
        string defaultOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
        var packages = Directory.GetDirectories(defaultOutputRoot);
        foreach (var packagePath in packages)
        {
            string package = Path.GetFileNameWithoutExtension(packagePath);
            var platforms = Directory.GetDirectories(packagePath);
            foreach (var platform in platforms) 
            {
                string path = $"{platform}/Simulate"; 
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
            }
        }
        GameConfig.Clear();
        Debug.Log($"编辑器运行模式缓存清理完成!!");
    }
}
