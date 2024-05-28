using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 发射口工厂
/// </summary>
public static class ShotFactory
{
    /// <summary> 红方发射口, key=所属发射口ID value=发射口数据 </summary>
    private static readonly Dictionary<int, AShotPointBase> redShotDict = new Dictionary<int, AShotPointBase>();
    
    /// <summary> 蓝方发射口, key=所属发射口ID value=发射口数据 </summary>
    private static readonly Dictionary<int, AShotPointBase> blueShotDict = new Dictionary<int, AShotPointBase>();

    public static AShotPointBase GetOrCreate(int id, bool isRed)
    {
        var shotPoint = GetOrCrateShotPoint(id);
        if (isRed) redShotDict[id] = shotPoint;
        else blueShotDict[id] = shotPoint;
        return shotPoint;
    }
    
    public static void Recycle(AShotPointBase item)
    {
        var id = item.ID;
        var isRed = item.IsRed;
        if (isRed)
            redShotDict.Remove(id);
        else
            blueShotDict.Remove(id);
        item.Clear();
        item?.Recycle();
    }

    public static AShotPointBase FindShotPoint(int id, bool isRed)
    {
        AShotPointBase shotPoint;
        if (isRed)
            redShotDict.TryGetValue(id, out shotPoint);
        else
            blueShotDict.TryGetValue(id, out shotPoint);
        if (shotPoint == null)
            Debuger.LogError($"{id}号发射器不存在！！");
        return shotPoint;
    }

    public static void Stop()
    {
        foreach (var kv in redShotDict)
            kv.Value.Stop();
        foreach (var kv in blueShotDict)
            kv.Value.Stop();
    }

    /// <summary>
    /// 角色上膛
    /// </summary>
    public static void OnPlayerShot(PlayerInfo info, TOperateData data, long count)
    {
        for (int i = 0; i < data.spwanKeys.Count; i++)
        {
            var key = data.spwanKeys[i];
            var value = data.GetValue(key);

            long num = value * count;
            var ball = TBallDataManager.Instance.GetItem(key);
            FindShotPoint(ball.firePoint, info.campType == CampType.红)?.OnShot(BallInfo.Create(ball, info), num, i == 0 ? data.headnum : 0, value);
        }
    }
    
    /// <summary>
    /// 角色上膛
    /// </summary>
    public static void OnShotBall(PlayerInfo info, int ballType, long count)
    {
        var ball = TBallDataManager.Instance.GetItemByType(ballType);
        FindShotPoint(ball.firePoint, info.campType == CampType.红)?.OnShot(BallInfo.Create(ball, info), count);
    }

    private static AShotPointBase GetOrCrateShotPoint(int id)
    {
        return id switch
        {
            1 => GameObjectFactory.GetOrCreate<ShotBombPoint>(),
            2 => GameObjectFactory.GetOrCreate<ShotGrindPoint>(),
            3 => GameObjectFactory.GetOrCreate<ShotHolePoint>(),
            4 => GameObjectFactory.GetOrCreate<ShotSmallPoint>(),
            5 => GameObjectFactory.GetOrCreate<ShotBigPoint>(),
            6 => GameObjectFactory.GetOrCreate<ShotPillPoint>(),
            _ => null
        };
    }

    public static void Register()
    {
        GameObjectFactory.Register<ShotSmallPoint>(PathConst.GetBattleItemPath(BattleConst.发射点_简单小球));
        GameObjectFactory.Register<ShotBigPoint>(PathConst.GetBattleItemPath(BattleConst.发射点_简单大球));
        GameObjectFactory.Register<ShotBombPoint>(PathConst.GetBattleItemPath(BattleConst.发射点_爆炸球球));
        GameObjectFactory.Register<ShotPillPoint>(PathConst.GetBattleItemPath(BattleConst.发射点_药丸球球));
        GameObjectFactory.Register<ShotHolePoint>(PathConst.GetBattleItemPath(BattleConst.发射点_黑洞球球));
        GameObjectFactory.Register<ShotGrindPoint>(PathConst.GetBattleItemPath(BattleConst.发射点_碾压球球));
    }
} 
