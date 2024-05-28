using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

public static class BallEffectFactory
{
    public static readonly Dictionary<GameObject, ABallEffectBase> ballDict =
        new Dictionary<GameObject, ABallEffectBase>();

    public static ABallEffectBase GetOrCreate(bool isRed, int effectType, Vector3 point, float effectTime = 0)
    {
        var ballEffect = isRed ? GetOrCreate_RED(effectType) : GetOrCreate_BLUE(effectType);
        ballEffect.OnInit(point, effectTime);
        var obj = ballEffect.GetGameObject();
        ballDict[obj] = ballEffect;
        return ballEffect;
    }

    public static void Recycle<T>(T item) where T : ABallEffectBase, new()
    {
        if (item == null) return;
        var obj = item.GetGameObject();
        ballDict.Remove(obj);
        GameObjectFactory.Recycle(item);
    }

    public static T Find<T>(GameObject obj) where T : ABallEffectBase, new()
    {
        ballDict.TryGetValue(obj, out var ball);
        return ball as T;
    }

    public static ABallEffectBase Find(GameObject obj)
    {
        ballDict.TryGetValue(obj, out var ball);
        return ball;
    }

    public static ABallEffectBase GetOrCreate_BLUE(int type)
    {
        return type switch
        {
            1 => GameObjectFactory.GetOrCreate<BlueDjPengZhuangEffect>(), //低级碰撞
            2 => GameObjectFactory.GetOrCreate<BlueGjPengZhuangEffect>(), //高级碰撞
            3 => GameObjectFactory.GetOrCreate<BlueTianTianQuanBoomEffect>(), //甜甜圈
            4 => GameObjectFactory.GetOrCreate<BlueHeiDongEffect>(), //黑洞
            5 => GameObjectFactory.GetOrCreate<BlueJinJIeEffect>(), //进阶
            _ => null
        };
    }

    public static ABallEffectBase GetOrCreate_RED(int type)
    {
        return type switch
        {
            1 => GameObjectFactory.GetOrCreate<RedDjPengZhuangEffect>(), //低级碰撞
            2 => GameObjectFactory.GetOrCreate<RedGjPengZhuangEffect>(), //高级碰撞
            3 => GameObjectFactory.GetOrCreate<RedTianTianQuanBoomEffect>(), //甜甜圈
            4 => GameObjectFactory.GetOrCreate<RedHeiDongEffect>(), //黑洞
            5 => GameObjectFactory.GetOrCreate<RedJinJieEffect>(), //进阶
            _ => null
        };
    }

    /// <summary>
    /// 一定要注册才能使用
    /// </summary>
    public static void Register()
    {
        GameObjectFactory.Register<RedDjPengZhuangEffect>(PathConst.GetBattleItemPath(BattleConst.低级碰撞_红));
        GameObjectFactory.Register<RedGjPengZhuangEffect>(PathConst.GetBattleItemPath(BattleConst.高级碰撞_红));
        GameObjectFactory.Register<RedTianTianQuanBoomEffect>(PathConst.GetBattleItemPath(BattleConst.甜甜圈爆_红));
        GameObjectFactory.Register<RedHeiDongEffect>(PathConst.GetBattleItemPath(BattleConst.黑洞_红));
        GameObjectFactory.Register<RedJinJieEffect>(PathConst.GetBattleItemPath(BattleConst.进阶_红));

        GameObjectFactory.Register<BlueDjPengZhuangEffect>(PathConst.GetBattleItemPath(BattleConst.低级碰撞_蓝));
        GameObjectFactory.Register<BlueGjPengZhuangEffect>(PathConst.GetBattleItemPath(BattleConst.高级碰撞_蓝));
        GameObjectFactory.Register<BlueTianTianQuanBoomEffect>(PathConst.GetBattleItemPath(BattleConst.甜甜圈爆_蓝));
        GameObjectFactory.Register<BlueHeiDongEffect>(PathConst.GetBattleItemPath(BattleConst.黑洞_蓝));
        GameObjectFactory.Register<BlueJinJIeEffect>(PathConst.GetBattleItemPath(BattleConst.进阶_蓝));
    }

    public static void RecycleAll()
    {
        var list = new List<ABallEffectBase>(ballDict.Values);
        foreach (var item in list)
            item?.Recycle();
        ballDict.Clear();
    }
}