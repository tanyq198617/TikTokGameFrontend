using System;
using System.Collections.Generic;
using Sproto;
using SprotoType;
using UnityEngine;

public class RankModel : Singleton<RankModel>
{
    /// <summary> 排行榜数据, key=排行榜类型, value=排行榜数据 </summary>
    private readonly Dictionary<long, List<RankInfo>> rankDict = new Dictionary<long, List<RankInfo>>();

    public float delay { get; private set; }

    /// <summary>
    /// 获取排行榜数据
    /// </summary>
    public List<RankInfo> GetRankList(int type)
    {
        rankDict.TryGetValue(type, out var list);
        return list;
    }
    
    /// <summary>
    /// 获取排行榜数据
    /// </summary>
    public List<RankInfo> GetRankListWithoutThree(int type)
    {
        var rankList = new List<RankInfo>();
        rankDict.TryGetValue(type, out var list);
        if (list.Count > 3)
            rankList.AddRange(list.GetRange(3, list.Count - 3));
        return rankList;
    }

    /// <summary>
    /// 获取某一个序列的数据
    /// </summary>
    /// <param name="type">排行榜类型</param>
    /// <param name="index">要获取的下标</param>
    /// <returns></returns>
    public RankInfo FindRankInfo(int type, int index)
    {
        if (rankDict.TryGetValue(type, out var list))
            return list.Count > index ? list[index] : null;
        return null;
    }

    #region 服务器通信
    
    /// <summary>
    /// 开始获取排行榜数据
    /// </summary>
    public void S2C_UpdateRankStart(long rankType)
    {
        if (!rankDict.TryGetValue(rankType, out var list))
            rankDict[rankType] = new List<RankInfo>();
        else
            ClearList(rankDict[rankType]);
    }

    /// <summary>
    /// 持续获取排行榜数据
    /// </summary>
    public void S2C_UpdateRank(long rankType, List<rank_info> list)
    {
        if (rankDict.TryGetValue(rankType, out _))
        {
            for (int i = 0; i < list.Count; i++)
            {
                rankDict[rankType].Add(RankInfo.Create(list[i]));
                list[i].LogMessageMembers($"send_rank_data.request (rankType={rankType})");
            }
        }
    }
    
    /// <summary>
    /// 结束获取排行榜数据
    /// </summary>
    public void S2C_UpdateRankEnd(long rankType)
    {                               
        //广播【结束获取排行榜数据】，此时刷新界面
        if (rankDict.TryGetValue(rankType, out var rankList))
            rankList.Sort(SortByRank);
    }
    
    /// <summary>
    /// 世界榜获取延迟时间
    /// 家族榜获取延迟时间
    /// </summary>
    /// <param name="delay"></param>
    public void SetDelay(float delay)
    {
        this.delay = delay;
    }

    #endregion
    
    /// <summary>
    /// 是否请求过排行榜数据
    /// </summary>
    public bool IsReqRank(int rankType)
    {
        rankDict.TryGetValue(rankType, out var rankList);
        return rankList != null && rankList.Count > 0;
    }

    private int SortByRank(RankInfo r1, RankInfo r2)
    {
        if (r1.rank < r2.rank)
            return -1;
        if (r1.rank > r2.rank)
            return 1;
        return 1;
    }

    /// <summary>
    /// 清理回收数据
    /// </summary>
    private void ClearList(IList<RankInfo> list)
    {
        for (int i = 0; i < list.Count; i++)
            list[i].Clear();
        list.Clear();
    }

    public void Clear()
    {
        foreach (var kv in rankDict)
        {
            ClearList(kv.Value);
        }
        rankDict.Clear();
    }
}

public enum ENUM_RANK_TYPE //1-世界榜 2-房间榜 3-本局榜 4-家族榜 5-连胜榜
{
    World = 1,
    Room = 2,
    Game = 3,
    Family = 4,
    winningStreak = 5,
}