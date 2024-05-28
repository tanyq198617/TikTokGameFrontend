using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红蓝方三个大哥的头像
/// </summary>
public class BattleCenterRichManPage : AItemPageBase
{
    private GameObject redRichManHeadItem;
    private GameObject blueRichManHeadItem;

    /// <summary> key=阵营; value={ key=位置点,value=数据 } </summary>
    private Dictionary<int, Dictionary<int, BattleRichManHeadItem>> headItemDic;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        headItemDic = new Dictionary<int, Dictionary<int, BattleRichManHeadItem>>();
        redRichManHeadItem = UIUtility.Control("uiitem_redplayerHeadObject", m_gameobj);
        redRichManHeadItem.SetActiveEX(false); 
        blueRichManHeadItem = UIUtility.Control("uiitem_blueplayerHeadObject", m_gameobj);
        blueRichManHeadItem.SetActiveEX(false);
        OnInit();
    }

    public override void Close()
    {
        base.Close();
        RecycleHeadItem();
    }

    /// <summary> 预加载生成6个大哥的头像脚本 /// </summary>
    private void OnInit()
    {
        for (int i = 1; i <= 2; i++)
        {
            headItemDic[i] = new Dictionary<int, BattleRichManHeadItem>();
            for (int j = 0; j <= 2; j++)
            {
                BattleRichManHeadItem item;
                if (i == 1)
                    item = UIUtility.CreateItem<BattleRichManHeadItem>(redRichManHeadItem, RectTrans);
                else
                    item = UIUtility.CreateItem<BattleRichManHeadItem>(blueRichManHeadItem, RectTrans);
                headItemDic[i][j] = item;
            }
        }
    }

    //刷新红蓝房的三个大哥的显示
    private void RefreshRichMan(CampType campType, List<PlayerInfo> playerInfos)
    {
        var key = campType.ToInt();
        if (headItemDic.TryGetValue(key, out var dict))
        {
            for (int i = 0; i < 3; i++)
            {
                if (dict.TryGetValue(i, out var sit))
                {
                    if (i < playerInfos.Count)
                    {
                        if (sit.IsActive)
                            sit?.SetPlayerInfo(playerInfos[i], i + 1);
                        else
                        {
                            sit.IsActive = true;
                            sit?.SetPlayerInfo(playerInfos[i], i + 1);
                        }
                    }
                    else
                    {
                        sit.IsActive = false;
                    }
                }
            }
        }
    }

    private void SetRichManHeadLocalPosition(bool _boo)
    {
        foreach (var VARIABLE in headItemDic.Values)
        {
            foreach (var value in VARIABLE.Values)
            {
                value.SetHeadMaskOffset(_boo);
            }
        }
    }
    
    /// <summary> 回收3巨头的头像 /// </summary>
    private void RecycleHeadItem()
    {
        if (headItemDic == null)
            return;
        foreach (var kv in headItemDic.Values)
        {
            foreach (var item in kv.Values)
            {
                item.IsActive = false;
            }
        }
    }

    /// <summary>监听游戏开始,回收3巨头的头像/// </summary>
    private void GameStart()
    {
        RecycleHeadItem();
    }

    #region 注册,释放event

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<CampType, List<PlayerInfo>>(BattleEvent.Battle_RickMan_Changed, RefreshRichMan);
        EventMgr.AddEventListener(BattleEvent.Battle_GameIsStart, GameStart);
        EventMgr.AddEventListener<bool>(BattleEvent.Battle_Camera_Rotation, SetRichManHeadLocalPosition);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<CampType, List<PlayerInfo>>(BattleEvent.Battle_RickMan_Changed, RefreshRichMan);
        EventMgr.RemoveEventListener(BattleEvent.Battle_GameIsStart, GameStart);
        EventMgr.RemoveEventListener<bool>(BattleEvent.Battle_Camera_Rotation, SetRichManHeadLocalPosition);
    }

    #endregion
}