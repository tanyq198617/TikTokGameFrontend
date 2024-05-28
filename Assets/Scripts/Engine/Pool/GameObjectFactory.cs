using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameObjectFactory
{
    private static readonly Dictionary<string, GameObject> PrefabDict = new Dictionary<string, GameObject>();

    public static Transform defaultRoot;
    
    public static T GetOrCreate<T>() where T : IGameObject, new() => GameObjectPool<T>.Instance.GetOrCreate();
    public static void RecycleAll<T>() where T : IGameObject, new() => GameObjectPool<T>.Instance.RecycleAll();
    public static void Recycle<T>(T t) where T : IGameObject, new() => GameObjectPool<T>.Instance.Recycle(t);

    public static void DestroyAll<T>() where T : IGameObject, new() => GameObjectPool<T>.Instance.DestroyAll();

    public static void Register<T>(string resPath, bool isDefault = true) where T : IGameObject, new()
        => GameObjectPool<T>.Instance.Init(resPath, isDefault ? defaultRoot : null);

    public static void initialize()
    {
        if (defaultRoot == null)
        {
            var go = new GameObject("~root");
            defaultRoot = go.transform; 
            Object.DontDestroyOnLoad(defaultRoot);
        }
    }

    public static void PreLoad<T>(string resPath, int count = 1) where T : IGameObject, new()
    {
        Register<T>(resPath);
        GameObjectPool<T>.Instance.PreLoad(count);
    }
    
    public static async UniTask PreLoadAsync<T>(string resPath, int count = 1) where T : IGameObject, new()
    {
        Register<T>(resPath);
        await GameObjectPool<T>.Instance.PreLoadAssets(count);
    }
    
    /// <summary>
    /// 一定要注册才能使用
    /// </summary>
    public static void Register()
    {
        initialize();
        Register<MuzzleControl>(PathConst.GetBattleItemPath(BattleConst.发射器_通用));
        Register<RedShotPointEffect>(PathConst.GetBattleItemPath(BattleConst.发射点_特效_红));
        Register<BlueShotPointEffect>(PathConst.GetBattleItemPath(BattleConst.发射点_特效_蓝));
        Register<BlueSmallExplosionEffect>(PathConst.GetBattleItemPath(BattleConst.组合球_小爆破_蓝));
        Register<RedSmallExplosionEffect>(PathConst.GetBattleItemPath(BattleConst.组合球_小爆破_红));
        
        Register<BlueSmallExplosionTrail>(PathConst.GetBattleItemPath(BattleConst.组合球_小爆破拖尾_蓝));
        Register<RedSmallExplosionTrail>(PathConst.GetBattleItemPath(BattleConst.组合球_小爆破拖尾_红));
        
        Register<RedThunderboltEffect>(PathConst.GetBattleItemPath(BattleConst.组合球_雷霆_红));
        Register<BlueThunderboltEffect>(PathConst.GetBattleItemPath(BattleConst.组合球_雷霆_蓝));
       
        Register<BlueInterludeAnimationEffect>(PathConst.GetBattleItemPath(BattleConst.大哥退场_蓝_TimeLine));
        Register<RedInterludeAnimationEffect>(PathConst.GetBattleItemPath(BattleConst.大哥退场_红_TimeLine));
        
        BallFactory.Register();
        ShotFactory.Register();
        BallEffectFactory.Register();
        FairyDragonEffectFactory.Register();
        GiftTimeLineFactory.Register();
        BallTimeLineFactory.Register();
    }

    #region 日志相关
    public static bool IsLog(Type t)
    {
        return !LogHashSets.IsNullOrNoCount() && LogHashSets.Contains(t);
    }

    public static HashSet<Type> LogHashSets = new HashSet<Type>
    {
        //typeof(HumanEntity),
        //typeof(DropItem)
    };
    #endregion
   

    #region 查找预制体
    public static GameObject FindPrefab(string resPath)
    {
        if (PrefabDict.TryGetValue(resPath, out var gameObject))
        {
            if (gameObject == null)
            {
                PrefabDict.Remove(resPath);
                return null;
            }    
            return gameObject;
        }
        return null;
    }

    public static void AddToPrefab(string resPath, GameObject gameObject)
    {
        if (gameObject == null) return;
        if (!PrefabDict.ContainsKey(resPath))
            PrefabDict.Add(resPath, gameObject);
    }
    #endregion

}
