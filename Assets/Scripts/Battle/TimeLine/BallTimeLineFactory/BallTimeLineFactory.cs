using System.Collections.Generic;
using UnityEngine;

/// <summary>球球timeline控制工厂/// </summary>
public static class BallTimeLineFactory
{
    /// <summary> 当前在播的所有timeLine字典 /// </summary>
    private static readonly Dictionary<GameObject, ATimeLineBase> timeLineDic =
        new Dictionary<GameObject, ATimeLineBase>();

    /// <summary> 等待播放的timeLine队列 /// </summary>
    private static Queue<TimeLineStruct> awaitPlayTimeLineQueue = new Queue<TimeLineStruct>();

    private static List<ATimeLineBase> timeLineIngList = new List<ATimeLineBase>();

    public static void GetOrCreate(PlayerInfo info, int effectType)
    {
        if (info == null)
        {
            Debuger.LogError("TimeLineFactory,玩家数据为空");
            return;
        }

        //当前有播放数据,加入队列
        if (timeLineIngList.Count > 0)
            awaitPlayTimeLineQueue.Enqueue(new TimeLineStruct(info, effectType));
        else
        {
            PlayTimeLine(info, effectType);
        }
    }

    private static ATimeLineBase PlayTimeLine(PlayerInfo info, int effectType)
    {
        var timeLineBase = info.campType == CampType.红 ? GetOrCreate_RED(effectType) : GetOrCreate_BLUE(effectType);
        if (timeLineBase == null)
        {
            Debuger.LogError($"没有找到TimeLine脚本,timeLine填表类型{effectType}");
            CheckAwaitPlayTimeLineQueue();
            return null;
        }
        timeLineIngList.Add(timeLineBase);
        return timeLineBase;
    }

    /// <summary> 回收上个timeline是检测一次待播队列里面是否还有数据 /// </summary>
    private static void CheckAwaitPlayTimeLineQueue()
    {
        if (awaitPlayTimeLineQueue.Count <= 0)
            return;
        else
        {
            var timeline = awaitPlayTimeLineQueue.Dequeue();
            PlayTimeLine(timeline.info, timeline.timeLineType);
        }
    }

    public static void Recycle<T>(T item) where T : ATimeLineBase, new()
    {
        if (item == null) return;
        GameObjectFactory.Recycle(item);
        timeLineIngList.Remove(item);
        CheckAwaitPlayTimeLineQueue();
    }

    private static ATimeLineBase GetOrCreate_BLUE(int type)
    {
        return type switch
        {
            1 => GameObjectFactory.GetOrCreate<BlueNianYaQiuJinJieTimeLine>(), //碾压球进阶_蓝_TimeLine
            2 => GameObjectFactory.GetOrCreate<BlueNianYaQiuRuChangTimeLine>(), //碾压球入场_蓝_TimeLine
            _ => null
        };
    }

    private static ATimeLineBase GetOrCreate_RED(int type)
    {
        return type switch
        {
            1 => GameObjectFactory.GetOrCreate<RedNianYaQiuJinJieTimeLine>(), //碾压球进阶_红_TimeLine
            2 => GameObjectFactory.GetOrCreate<RedNianYaQiuRuChangTimeLine>(), //碾压球入场_红_TimeLine
            _ => null
        };
    }


    /// <summary>
    /// 一定要注册才能使用
    /// </summary>
    public static void Register()
    {
        GameObjectFactory.Register<RedNianYaQiuJinJieTimeLine>(PathConst.GetBattleItemPath(BattleConst.碾压球进阶_红_TimeLine));
        GameObjectFactory.Register<RedNianYaQiuRuChangTimeLine>(PathConst.GetBattleItemPath(BattleConst.碾压球入场_红_TimeLine));

        GameObjectFactory.Register<BlueNianYaQiuJinJieTimeLine>(PathConst.GetBattleItemPath(BattleConst.碾压球进阶_蓝_TimeLine));
        GameObjectFactory.Register<BlueNianYaQiuRuChangTimeLine>(PathConst.GetBattleItemPath(BattleConst.碾压球入场_蓝_TimeLine));
    }

    /// <summary> 回收所有正在播的timeline特效 /// </summary>
    public static void RecycleAll()
    {
        awaitPlayTimeLineQueue.Clear();
        for (int i = timeLineIngList.Count - 1; i >= 0; i--)
            timeLineIngList[i].Recycle();
        timeLineIngList.Clear();
    }
}
