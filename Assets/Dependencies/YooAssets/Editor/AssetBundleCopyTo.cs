using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;

public class AssetBundleCopyTo
{
    private const string OutPutDir = "增量资源";

    [MenuItem("YooAsset/拷贝增量资源到", false, 402)]
    public static async void CopyTo()
    {
        string defaultOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();

        var platforms = Directory.GetDirectories(defaultOutputRoot);

        EditorUtility.DisplayProgressBar("复制", "准备复制中...", 0f);

        foreach (var platformsPath in platforms)
        {
            string platform = Path.GetFileNameWithoutExtension(platformsPath);

            if (platform.Equals(EditorUserBuildSettings.activeBuildTarget.ToString()))
            {
                var packages = Directory.GetDirectories(platformsPath);

                foreach (var package in packages)
                {
                    string name = Path.GetFileNameWithoutExtension(package);

                    var assetBundleDirs = Directory.GetDirectories(package);

                    DateTime time = default;
                    string select = null;
                    foreach (var dir in assetBundleDirs)
                    {
                        if (dir.Contains("Simulate"))
                            continue;
                        var dateTime = Directory.GetCreationTime(dir);
                        if (time < dateTime)
                        {
                            time = dateTime;
                            select = dir;
                        }
                    }

                    string pkname = Path.GetFileNameWithoutExtension(select);

                    //通过配置，获得打包开始时间，凡是高于这个时间的属于增量更新
                    var _reportFilePath = $"{package}/{pkname}/BuildReport_{name}_{pkname}.json";
                    string jsonData = File.ReadAllText(_reportFilePath);
                    var _buildReport = BuildReport.Deserialize(jsonData);
                    var _report = _buildReport.Summary;
                    time = Convert.ToDateTime(_report.BuildDate);
                    time = time.AddSeconds(-_report.BuildSeconds);

                    bool isAll = _report.BuildMode == 0;

                    string outPutPath = $"{defaultOutputRoot}/{OutPutDir}/{pkname}/";

                    if (Directory.Exists(outPutPath))
                        Directory.Delete(outPutPath, true);
                    Directory.CreateDirectory(outPutPath);

                    var dirInfo = new DirectoryInfo($"{package}/{pkname}");
                    var files = dirInfo.GetFiles();

                    //Debug.LogError($"{select}  文件个数：{files.Length}  {time.ToString("yyyy-MM-dd HH-mm-ss")}");
                    int i = 0;
                    foreach (var file in files)
                    {
                        i++;
                        if (isAll || file.LastWriteTime >= time)
                        {
                            File.Copy(file.FullName, $"{outPutPath}{file.Name}", true);
                            EditorUtility.DisplayProgressBar("复制", $"复制{pkname}中.第{i}/{files.Length}个",
                                i * 1.0f / files.Length);
                        }

                        await UniTask.Yield();
                    }
                }
            }
        }

        EditorUtility.ClearProgressBar();
        Debug.Log($"拷贝完毕");
    }
}