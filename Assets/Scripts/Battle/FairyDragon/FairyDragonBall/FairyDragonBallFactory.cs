using System;
using System.Collections.Generic;
using System.Linq;
using HotUpdateScripts;
using Org.BouncyCastle.Utilities.Zlib;

/// <summary>神龙炸弹工厂/// </summary>
public static class FairyDragonBallFactory
{
    /// <summary> 正在播放的神龙字典,最多存红蓝两个 /// </summary>
    private static readonly Dictionary<CampType, FairyDragonBall> fairDragonBallDic = new Dictionary<CampType, FairyDragonBall>();

    /// <summary> 红蓝方缓存为播神龙炸弹的数据 /// </summary>
    private static readonly Dictionary<CampType, Queue<PlayerInfo>> _cachingPlayerInfoDic =
        new Dictionary<CampType, Queue<PlayerInfo>>()
        {
            [CampType.红] = new Queue<PlayerInfo>(),
            [CampType.蓝] = new Queue<PlayerInfo>()
        };

    /// <summary> 创建一个神龙炸弹实例脚本 /// </summary>
    public static void GetOrCreate(PlayerInfo info)
    {
        if (CityGlobal.Instance.IsOver) return;
        if (IsPlayFairyDragonBall(info.campType))
            PlayFairyDragon(info);
        else
        {
            if (_cachingPlayerInfoDic.TryGetValue(info.campType, out var value))
            {
                value.Enqueue(info);
            }
        }
    }

    /// <summary>判断是否播放神龙炸弹/// </summary>
    private static bool IsPlayFairyDragonBall(CampType camp)
    {
        return !fairDragonBallDic.TryGetValue(camp, out _);
    }

    /// <summary> 播放神龙炸弹 /// </summary>
    private static void PlayFairyDragon(PlayerInfo info)
    {
        var ball = GetOrCreate();
        ball.OnInit(info);
        fairDragonBallDic[info.campType] = ball;
    }

    /// <summary> 当前阵营,检测是否有缓存待播的数据 /// </summary>
    public static bool CheckPlayFairyDragon(CampType camp)
    {
        if (!_cachingPlayerInfoDic.TryGetValue(camp, out var value)) return true;
        if (value.Count <= 0) return true;
        var info = value.Dequeue();
        PlayFairyDragon(info);
        return false;
    }
    
    /// <summary> 回收单个神龙炸弹脚本 /// </summary>
    public static void Recycle(FairyDragonBall item)
    {
        if (item == null) return;
        var _camp = item.info.campType;
        item.Recycle();
        ClassFactory.Recycle(item);
        
        if (item.isCloseBall)
        {
            fairDragonBallDic.Remove(_camp);
            CheckPlayFairyDragon(_camp);
        }
    }

    /// <summary> 获取神龙炸弹脚本 /// </summary>
    private static FairyDragonBall GetOrCreate()
    {
        return ClassFactory.GetOrCreate<FairyDragonBall>();;
    }

    /// <summary> 清除场上所有神龙炸弹脚本 /// </summary>
    public static void RecycleAll()
    {
        _cachingPlayerInfoDic[CampType.红].Clear();
        _cachingPlayerInfoDic[CampType.蓝].Clear();
        var balls = fairDragonBallDic.Values.ToList();
        for (var i = balls.Count - 1; i >= 0; i--)
        {
            balls[i].Recycle();
        }
        fairDragonBallDic.Clear();
    }
}