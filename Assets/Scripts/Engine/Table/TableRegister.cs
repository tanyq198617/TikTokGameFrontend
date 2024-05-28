using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class TableRegister
{
    protected static readonly Dictionary<string, byte[]> handlerDict = new Dictionary<string, byte[]>();

    public static bool IsLoadingAll = false;

    public static async UniTaskVoid PreLoadAll()
    {
        IsLoadingAll = false;
        var tableInfos = YooAssets.GetAssetInfos("table");
        WatchTime.Start();
        List<UniTask> tasks = new List<UniTask>();
        foreach (var tableInfo in tableInfos)
        {
            tasks.Add(LoadTable(tableInfo));
        }
        await UniTask.WhenAll(tasks);
        IsLoadingAll = true;
        tasks.Clear();
        tasks = null;
        // WatchTime.ShowTime($"加载所有表格资源, 预计{tableInfos.Length}个, 实际{handlerDict.Count}个, 耗时：");
    }

    private static async UniTask LoadTable(AssetInfo info)
    {
        var handle = YooAssets.LoadAssetAsync(info);
        await handle.ToUniTask();
        var asset = handle.GetAssetObject<TextAsset>();
        handlerDict[info.AssetPath] = new byte[asset.bytes.Length];
        Buffer.BlockCopy(asset.bytes, 0, handlerDict[info.AssetPath], 0, asset.bytes.Length);
        handle.Release();
    }

    public static byte[] GetTextAsset(string path)
    {
        handlerDict.TryGetValue(path, out var bytes);
        return bytes;
    }

    public static void UnLoad(string path)
    {
        handlerDict.Remove(path);
    }
}
