using System.Collections.Generic;
using System.Linq;
using HotUpdateScripts;
using UnityEngine;

public class DaGeRuChangPage : AItemPageBase
{
    private GameObject e2d_ui_dage;
    private GameObject e2d_ui_erge;
    private GameObject e2d_ui_sange;

    /// <summary> 还没有播大哥入场的玩家 /// </summary>
    private Queue<NoPlayDaGeRuChangStruct> playerInfoQueue;

    private List<KingDataCfg> kingCfgList;

    /// <summary> 当前是否正在播大哥入场 /// </summary>
    private bool isPlayIng;

    /// <summary> 下面两个重点的key都是大哥入场表里面的ruchangPrefabType这个字段,一一对应 /// </summary>
    private Dictionary<int, GameObject> e2dDic;
    private Dictionary<int, Queue<DaGeRuChangTip>> ruchangTipQueue;

    private HashSet<DaGeRuChangTip> _hashSet;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        e2d_ui_dage = UIUtility.Control("E2D_UI_dage", m_gameobj);
        e2d_ui_erge = UIUtility.Control("E2D_UI_erge", m_gameobj);
        e2d_ui_sange = UIUtility.Control("E2D_UI_sange", m_gameobj);
        e2dDic = new Dictionary<int, GameObject>();
        e2dDic[1] = e2d_ui_dage;
        e2dDic[2] = e2d_ui_erge;
        e2dDic[3] = e2d_ui_sange;
        kingCfgList = TGlobalDataManager.Instance.kingCfgList;
        
        ruchangTipQueue = new Dictionary<int, Queue<DaGeRuChangTip>>();
        ruchangTipQueue[1] = new Queue<DaGeRuChangTip>();
        ruchangTipQueue[2] = new Queue<DaGeRuChangTip>();
        ruchangTipQueue[3] = new Queue<DaGeRuChangTip>();
        playerInfoQueue = new Queue<NoPlayDaGeRuChangStruct>();

        _hashSet = new HashSet<DaGeRuChangTip>();
        isPlayIng = false;
    }

    public override void Refresh()
    {
        base.Refresh();
        isPlayIng = false;
    }

    public override void Close()
    {
        base.Close();

        foreach (var item in _hashSet)
        {
            
            if (ruchangTipQueue.TryGetValue(item.ruChangStruct.dataCfg.ruchangPrefabType, out var _queue))
                _queue.Enqueue(item);
            item.IsActive = false;
        }
        _hashSet.Clear();
        playerInfoQueue.Clear();
        isPlayIng = false;
    }

    public void RefreshDaGeTip(PlayerInfo info)
    {
        bool _bool = IsAddPlayerInfoQueue(info);
        if (_bool)
        {
            if (!isPlayIng)
            {
                PlayDaGeRuChang();
            }
        }
    }

    /// <summary> 玩家是否有资格播大哥入场 /// </summary>
    private bool IsAddPlayerInfoQueue(PlayerInfo info)
    {
        if (info.world_rank <= 0)
            return false;
        for (int i = 0; i < kingCfgList.Count(); i++)
        {
            if (info.world_rank < kingCfgList[i].rankId)
            {
                playerInfoQueue.Enqueue(new NoPlayDaGeRuChangStruct(info, kingCfgList[i]));
                return true;
            }
        }

        return false;
    }

    /// <summary> 开始播放大哥入场 /// </summary>
    private void PlayDaGeRuChang()
    {
        if (playerInfoQueue.Count <= 0)
        {
            isPlayIng = false;
            return;
        }

        isPlayIng = true;
        NoPlayDaGeRuChangStruct _struct = playerInfoQueue.Dequeue();
        DaGeRuChangTip tip = GetRuChangTip(_struct.dataCfg.ruchangPrefabType);
        tip.IsActive = true;
        tip.RefreshContent(_struct,RecycleSide);
        _hashSet.Add(tip);
    }

    /// <summary> 获取大哥入场的脚本 /// </summary>
    private DaGeRuChangTip GetRuChangTip(int type)
    {
        Queue<DaGeRuChangTip> _queue = ruchangTipQueue[type];
        if (_queue.Count > 0)
        {
            return _queue.Dequeue();
        }
        else
        {
            var tip = UIUtility.CreatePage<DaGeRuChangTip>(e2dDic[type], Trans);
            tip.daGetype = type;
            return tip;
        }
    }


    /// <summary> 回收玩家播放送礼完成后的预制体和脚本 /// </summary>
    private void RecycleSide(DaGeRuChangTip tip)
    {
        isPlayIng = false;

        if (ruchangTipQueue.TryGetValue(tip.ruChangStruct.dataCfg.ruchangPrefabType, out var _queue))
            _queue.Enqueue(tip);
        tip.IsActive = false;
        PlayDaGeRuChang();
        _hashSet.Remove(tip);
    }
}

/// <summary> 等待播放打个入场的结构体 /// </summary>
public struct NoPlayDaGeRuChangStruct
{
    public PlayerInfo playerInfo;
    public KingDataCfg dataCfg;

    public NoPlayDaGeRuChangStruct(PlayerInfo _info, KingDataCfg _cfg)
    {
        playerInfo = _info;
        dataCfg = _cfg;
    }
}