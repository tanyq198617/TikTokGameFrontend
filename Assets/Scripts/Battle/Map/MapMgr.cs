using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class MapMgr : Singleton<MapMgr>
{
    private AssetOperationHandle mapHandler;
    private MapControl Map;

    public GameObject gameobject_xuetiao_red;
    public GameObject gameobject_xuetiao_lan;
    public async UniTask LoadMap()
    {
        if (Map == null)
        {
            string path = PathConst.GetBattleItemPath(BPath.MapItem);

            if (!YooAssets.CheckLocationValid(path))
            {
                Debuger.LogError($"不存在的地图资源路径：{path}");
                return;
            }
            mapHandler ??= YooAssets.LoadAssetAsync<GameObject>(path);
            await mapHandler.ToUniTask();
            var prefab = mapHandler.GetAssetObject<GameObject>();
            Map = UIUtility.CreatePage<MapControl>(prefab, null);
        }

        var leveledit = GameObject.Find("level edit");
        gameobject_xuetiao_red = UIUtility.Control("xuetiao_red", leveledit);
        gameobject_xuetiao_lan = UIUtility.Control("xuetiao_lan", leveledit);
    }

    public void Initialize()
    {
        if (Map != null)
            Map.IsActive = true;
        gameobject_xuetiao_red?.SetActiveEX(true);
        gameobject_xuetiao_lan?.SetActiveEX(true);
    }

    public void Close()
    {
        if (Map != null)
            Map.IsActive = false;
        gameobject_xuetiao_red?.SetActiveEX(true);
        gameobject_xuetiao_lan?.SetActiveEX(true);
    }

    public void OnDestory()
    {
        Close();
        Map?.Clear();
        Map?.Destory();
        Map = null;
        
        mapHandler?.Release();
        mapHandler = null;
    }
}
