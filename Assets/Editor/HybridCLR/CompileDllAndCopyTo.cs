using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CompileDllAndCopyTo
{
    private static string dstDir = "Assets/HotUpdateResources/Codes";

    [MenuItem("HybridCLR/Build/CompileDllAndCopyTo _F6")]
    public static void BuildAndCopyABAOTHotUpdateDlls()
    {
        if (!Directory.Exists(dstDir))
            Directory.CreateDirectory(dstDir);
        EditorUtility.DisplayProgressBar("编译", "正在编译DLL中", 0.3f);
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        CompileDllCommand.CompileDll(target);
        EditorUtility.DisplayProgressBar("拷贝", "正在拷贝到Assets/HotUpdateResources/Codes", 0.9f);
        CopyABAOTHotUpdateDlls();
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        Debug.Log($"[编译Dll] 成功编译!!!");
    }

    public static void CopyABAOTHotUpdateDlls()
    {
        CopyAOTAssembliesToHotfixDir();
        CopyHotUpdateAssembliesToHotfixDir();
    }

    public static void CopyAOTAssembliesToHotfixDir()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;
        string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
        string aotAssembliesDstDir = dstDir;

        foreach (var dll in SettingsUtil.AOTAssemblyNames)
        {
            string srcDllPath = $"{aotAssembliesSrcDir}/{dll}.dll";
            if (!File.Exists(srcDllPath))
            {
                Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                continue;
            }
            string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.dll.bytes";
            File.Copy(srcDllPath, dllBytesPath, true);
            Debug.Log($"[编译Dll] Copy AOT Dll {srcDllPath} -> {dllBytesPath}");
        }
    }

    public static void CopyHotUpdateAssembliesToHotfixDir()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;

        string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
        string hotfixAssembliesDstDir = dstDir;
#if NEW_HYBRIDCLR_API
        foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
#else
            foreach (var dll in SettingsUtil.HotUpdateAssemblyFiles)
#endif
        {
            string dllPath = $"{hotfixDllSrcDir}/{dll}";
            string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.bytes";
            File.Copy(dllPath, dllBytesPath, true);
            Debug.Log($"[编译Dll] Copy Hotfix Dll {dllPath} -> {dllBytesPath}");
        }
    }
}
