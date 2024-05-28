using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 红蓝方各30个小弟的头像
/// </summary>
public class BattleYoungerBrotherPage:AItemPageBase
{
    private Queue<BattleYoungerBrotherHeadItem> headItemQueue;
    private Dictionary<int, Dictionary<int,BattleYoungerBrotherHeadItem>> headItemDict;
    private GameObject youngerBrotherHeadItem;
    
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        headItemQueue = new Queue<BattleYoungerBrotherHeadItem>();
        headItemDict = new Dictionary<int, Dictionary<int, BattleYoungerBrotherHeadItem>>();
        headItemDict[CampType.蓝.ToInt()] = new Dictionary<int, BattleYoungerBrotherHeadItem>();
        headItemDict[CampType.红.ToInt()] = new Dictionary<int, BattleYoungerBrotherHeadItem>();
        youngerBrotherHeadItem = UIUtility.Control("uiitem_playerHeadObject", m_gameobj);
        youngerBrotherHeadItem.SetActiveEX(false);
    }

    /// <summary>
    /// 增加一个位置
    /// </summary>
    private void RefreshYoungerBrother(int rank,PlayerInfo info)
    {
        if(CityGlobal.Instance.IsOver)
            return;
        
        var item = GetHeadItem();
        if(item == null) return;
        item.IsActive = true;
        item.SetPlayerInfo(rank,info);
   
        if (headItemDict.TryGetValue(info.campType.ToInt(), out var headItem))
        {
            headItem[rank] = item ;
        }
    }
    
    /// <summary>
    /// 替换一个数据
    /// </summary>
    private void ChangeYoungerBrother(int rank, PlayerInfo info)
    {
        if (headItemDict.TryGetValue(info.campType.ToInt(), out var headItem))
        {
            headItem[rank].SetPlayerInfo(rank, info);
        }
    }

    private BattleYoungerBrotherHeadItem GetHeadItem()
    {
        if (headItemQueue.Count > 0)
        {
            BattleYoungerBrotherHeadItem item = headItemQueue.Dequeue();
            return item;
        }

        BattleYoungerBrotherHeadItem createItem =  UIUtility.CreateItem<BattleYoungerBrotherHeadItem>(youngerBrotherHeadItem, Trans);
        return createItem;
    }

    /// <summary> 相机是否过中线,true过中线,false没过 /// </summary>
    private void SetHeadLocalPosition(bool _boo)
    {
        for (int i = 1; i <= headItemDict.Count; i++)
        {
            var list = new List<BattleYoungerBrotherHeadItem>(headItemDict[i].Values);
            for (int j = list.Count - 1; j >= 0; j--)
            {
                list[j].RefreshTargetPoint();
            }
        }
    }
    
    /// <summary>监听游戏开始,回收红蓝方30小弟的头像/// </summary>
    private void RecycleAll()
    {
       
        for (int i = 1; i <= headItemDict.Count; i++)
        {
            var list = new List<BattleYoungerBrotherHeadItem>(headItemDict[i].Values);
            for (int j = list.Count - 1; j >= 0; j--)
            {
            
                var item = list[j];
             
                item.IsActive = false;
                headItemQueue.Enqueue(item);
            }
            list.Clear();
        }
       
        headItemDict[CampType.蓝.ToInt()].Clear();
        headItemDict[CampType.红.ToInt()].Clear();
    }

    #region 注册,释放event

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<int,PlayerInfo>(BattleEvent.Battle_Add_YoungerBrother, RefreshYoungerBrother);
        EventMgr.AddEventListener<int,PlayerInfo>(BattleEvent.Battle_Changed_YoungerBrother, ChangeYoungerBrother);
        EventMgr.AddEventListener(BattleEvent.Battle_RestartGame, RecycleAll);
        EventMgr.AddEventListener<bool>(BattleEvent.Battle_Camera_Rotation, SetHeadLocalPosition);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<int,PlayerInfo>(BattleEvent.Battle_Add_YoungerBrother, RefreshYoungerBrother);
        EventMgr.RemoveEventListener<int,PlayerInfo>(BattleEvent.Battle_Changed_YoungerBrother, ChangeYoungerBrother);
        EventMgr.RemoveEventListener(BattleEvent.Battle_RestartGame, RecycleAll);
        EventMgr.AddEventListener<bool>(BattleEvent.Battle_Camera_Rotation, SetHeadLocalPosition);
    }
    #endregion
}