using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using YooAsset;

public class AtlasMgr : MonoBehaviour
{
    private readonly Dictionary<string, AssetOperationHandle> dict = new Dictionary<string, AssetOperationHandle>();
    
    private void Awake()
    {
        SpriteAtlasManager.atlasRequested += OnAtlasRequested;
        GameObject.DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        SpriteAtlasManager.atlasRequested -= OnAtlasRequested;

        foreach (var kv in dict)
            kv.Value.Release();
        dict.Clear();
    }

    private void OnAtlasRequested(string tag, Action<SpriteAtlas> action)
    {
        if (!dict.TryGetValue(tag, out var handler))
        {
            handler = YooAssets.LoadAssetSync<SpriteAtlas>(PathConst.GetAtlasPath(tag));
            handler.Completed += (handle) =>
            {
                var spriteAtlas = handle.GetAssetObject<SpriteAtlas>();
                if (action != null)
                    action(spriteAtlas);
            };
        }
        else
        {
            var spriteAtlas = handler.GetAssetObject<SpriteAtlas>();
            if (action != null)
                action(spriteAtlas);
        }
    }
}