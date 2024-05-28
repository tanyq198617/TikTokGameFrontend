using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// 球球工厂
/// </summary>
public static class BallFactory
{
    /// <summary> 缓存场上所有红方子弹脚本 /// </summary>
    public static readonly Dictionary<GameObject, ABallBase> redBallDict = new Dictionary<GameObject, ABallBase>();
    
    /// <summary> 缓存场上所有蓝方子弹脚本 /// </summary>
    public static readonly Dictionary<GameObject, ABallBase> blueBallDict = new Dictionary<GameObject, ABallBase>();
    
    /*/// <summary> 缓存场上所有子弹脚本 /// </summary>
    public static readonly Dictionary<GameObject, ABallBase> ballDict = new Dictionary<GameObject, ABallBase>();*/

    public static ABallBase GetOrCreate(bool isRed, TBallData ballData,  PlayerInfo player, bool isShowHead)
    {
        var ball = isRed ? GetOrCreate_RED(ballData.type) : GetOrCreate_BLUE(ballData.type);
        ball.OnInit(isRed, ballData, player, isShowHead);
        AddBallBaseToDic(ball);
        return ball;
    }

    public static void Recycle<T>(T item) where T : ABallBase, new()
    {
        if(item == null) return;
        var obj = item.GetGameObject();
        if(item.Camp == CampType.红.ToInt()) 
            redBallDict.Remove(obj);
        else 
            blueBallDict.Remove(obj);
        GameObjectFactory.Recycle(item);
    }

    /// <summary> 创建子弹时添加到红蓝方对应的字典中 /// </summary>
    private static void AddBallBaseToDic(ABallBase ball)
    {
        var obj = ball.GetGameObject();
        if (ball.Camp == CampType.红.ToInt())
            redBallDict[obj] = ball;
        else
            blueBallDict[obj] = ball;
    }
    
    public static T Find<T>(GameObject obj) where T : ABallBase, new()
    { 
        //ballDict.TryGetValue(obj, out var ball);
        if (redBallDict.TryGetValue(obj, out var redBall))
            return redBall as T;
        else
        {
            blueBallDict.TryGetValue(obj, out var blueBall);
            return blueBall as T;
        }
    }

    public static void Stop()
    {
        foreach (var kv in redBallDict)
        {
            kv.Value.Stop();
        }
        foreach (var kv in blueBallDict)
        {
            kv.Value.Stop();
        }
    }

    public static ABallBase Find(GameObject obj)
    {
        if (redBallDict.TryGetValue(obj, out var redBall))
            return redBall;
        if (blueBallDict.TryGetValue(obj, out var blueBall))
            return blueBall;
        return null;
    }

    public static ABallBase Find(CampType campType, GameObject obj)
    {
        if (campType == CampType.红)
        {
            redBallDict.TryGetValue(obj, out var ball);
            return ball;
        }
        else
        {
            blueBallDict.TryGetValue(obj, out var ball);
            return ball;
        }
    }

    public static List<ABallBase> GetAllBall(CampType campType)
    {
        var list = new List<ABallBase>();
        list.AddRange(campType == CampType.红 ? redBallDict.Values : blueBallDict.Values);
        return list;
    }

    public static void OnBuffChanged(CampType campType, BuffHandler handler)
    {
        var balls = GetAllBall(campType);
        for (int j = balls.Count - 1; j >= 0; j--)
            balls[j].OnBuffChanged(handler);
    }

    public static ABallBase GetOrCreate_BLUE(int type)
    {
        return type switch
        {
            1 => GameObjectFactory.GetOrCreate<BlueBombBall>(),     //爆炸球
            2 => GameObjectFactory.GetOrCreate<BlueGrindBall>(),    //碾压球
            3 => GameObjectFactory.GetOrCreate<BlueHoleBall>(),     //黑洞球
            4 => GameObjectFactory.GetOrCreate<BlueSimpleBall>(),   //简单小球
            5 => GameObjectFactory.GetOrCreate<BlueBigBall>(),      //简单大球
            6 => GameObjectFactory.GetOrCreate<BluePillBall>(),     //药丸球
            7 => GameObjectFactory.GetOrCreate<BlueDanzaiBall>(),   //蛋仔球
            8 => GameObjectFactory.GetOrCreate<BlueSmallBombBall>(),//小爆炸球
            _ => null
        };
    }
    
    public static ABallBase GetOrCreate_RED(int type)
    {
        return type switch
        {
            1 => GameObjectFactory.GetOrCreate<RedBombBall>(),      //爆炸球
            2 => GameObjectFactory.GetOrCreate<RedGrindBall>(),     //碾压球
            3 => GameObjectFactory.GetOrCreate<RedHoleBall>(),      //黑洞球
            4 => GameObjectFactory.GetOrCreate<RedSimpleBall>(),    //简单小球
            5 => GameObjectFactory.GetOrCreate<RedBigBall>(),       //简单大球
            6 => GameObjectFactory.GetOrCreate<RedPillBall>(),      //药丸球
            7 => GameObjectFactory.GetOrCreate<RedDanzaiBall>(),    //蛋仔球
            8 => GameObjectFactory.GetOrCreate<RedSmallBombBall>(),//小爆炸球
            _ => null
        };
    }
    
    /// <summary>
    /// 一定要注册才能使用
    /// </summary>
    public static void Register()
    {
        GameObjectFactory.Register<RedSimpleBall>(PathConst.GetBattleItemPath(BattleConst.球球_简单小球_红));
        GameObjectFactory.Register<RedBigBall>(PathConst.GetBattleItemPath(BattleConst.球球_简单大球_红));
        GameObjectFactory.Register<RedBombBall>(PathConst.GetBattleItemPath(BattleConst.球球_爆炸球球_红));
        GameObjectFactory.Register<RedPillBall>(PathConst.GetBattleItemPath(BattleConst.球球_药丸球球_红));
        GameObjectFactory.Register<RedHoleBall>(PathConst.GetBattleItemPath(BattleConst.球球_黑洞球球_红));
        GameObjectFactory.Register<RedGrindBall>(PathConst.GetBattleItemPath(BattleConst.球球_碾压球球_红));
        GameObjectFactory.Register<RedDanzaiBall>(PathConst.GetBattleItemPath(BattleConst.球球_蛋仔球_红));
        GameObjectFactory.Register<RedSmallBombBall>(PathConst.GetBattleItemPath(BattleConst.球球_小爆炸球_红));
        
        GameObjectFactory.Register<BlueSimpleBall>(PathConst.GetBattleItemPath(BattleConst.球球_简单小球_蓝));
        GameObjectFactory.Register<BlueBigBall>(PathConst.GetBattleItemPath(BattleConst.球球_简单大球_蓝));
        GameObjectFactory.Register<BlueBombBall>(PathConst.GetBattleItemPath(BattleConst.球球_爆炸球球_蓝));
        GameObjectFactory.Register<BluePillBall>(PathConst.GetBattleItemPath(BattleConst.球球_药丸球球_蓝));
        GameObjectFactory.Register<BlueHoleBall>(PathConst.GetBattleItemPath(BattleConst.球球_黑洞球球_蓝));
        GameObjectFactory.Register<BlueGrindBall>(PathConst.GetBattleItemPath(BattleConst.球球_碾压球球_蓝));
        GameObjectFactory.Register<BlueDanzaiBall>(PathConst.GetBattleItemPath(BattleConst.球球_蛋仔球_蓝));
        GameObjectFactory.Register<BlueSmallBombBall>(PathConst.GetBattleItemPath(BattleConst.球球_小爆炸球_蓝));
    }

    /// <summary> 播放场上所有球的死亡动画 /// </summary>
    public static void PlayBallDieTweenAll()
    {
        var list = new List<ABallBase>(redBallDict.Values);
        foreach (var item in list)
            item?.GameOverDieTween();
        var blueList = new List<ABallBase>(blueBallDict.Values);
        foreach (var item in blueList)
            item?.GameOverDieTween();
    }
    
    public static void RecycleAll()
    {
        var list = new List<ABallBase>(redBallDict.Values);
        foreach (var item in list)
            item?.Recycle();
        var blueList = new List<ABallBase>(blueBallDict.Values);
        foreach (var item in blueList)
            item?.Recycle();
        redBallDict.Clear();
        blueBallDict.Clear();
    }
}
