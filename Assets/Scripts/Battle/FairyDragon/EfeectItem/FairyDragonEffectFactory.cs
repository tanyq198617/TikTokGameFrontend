using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 神龙炸弹的两个出场,循环timeLine对象池工厂
/// 不需要在这个工厂里统一回收对象,
/// 回收神龙的控制脚本,达到回收对象池的功效
/// </summary>
public static class FairyDragonEffectFactory
{
    public static AFairyDragonEffectBase GetOrCreate(bool isRed)
    {
        var ballEffect = isRed ? GetOrCreate_RED() : GetOrCreate_BLUE();
        return ballEffect;
    }

    public static void Recycle<T>(T item) where T : AFairyDragonEffectBase, new()
    {
        if (item == null) return;
        var obj = item.GetGameObject();
        GameObjectFactory.Recycle(item);
    }

    public static AFairyDragonEffectBase GetOrCreate_BLUE()
    {
        return GameObjectFactory.GetOrCreate<BlueFairyDragonChuChangEffect>(); //蓝方神龙出场timeling
    }

    public static AFairyDragonEffectBase GetOrCreate_RED()
    {
        return GameObjectFactory.GetOrCreate<RedFairyDragonChuChangEffect>(); //红方神龙出场timeling
    }

    /// <summary>
    /// 一定要注册才能使用
    /// </summary>
    public static void Register()
    {
        GameObjectFactory.Register<RedFairyDragonChuChangEffect>(
            PathConst.GetBattleItemPath(BattleConst.RedFairyDragonChuChangTimeLine));

        GameObjectFactory.Register<BlueFairyDragonChuChangEffect>(
            PathConst.GetBattleItemPath(BattleConst.BlueFairyDragonChuChangTimeLine));
    }

}