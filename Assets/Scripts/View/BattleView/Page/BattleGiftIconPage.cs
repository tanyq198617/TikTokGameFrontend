using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>玩家送礼,礼物图标缩小旋转到玩家头像上/// </summary>
public class BattleGiftIconPage : AItemPageBase
{

    private Queue<PlayerGiftIconItem> giftTipQueue;

    private HashSet<PlayerGiftIconItem> _hashSet;

    private GameObject uiitem_giftIconItem;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        giftTipQueue = new Queue<PlayerGiftIconItem>();
        _hashSet = new HashSet<PlayerGiftIconItem>();
        uiitem_giftIconItem = UIUtility.Control("uiitem_giftIconItem", m_gameobj);
        uiitem_giftIconItem.SetActiveEX(false);
    }

    private void RefreshGiftTip(string openId, string giftId, long giftCount)
    {
        var info = PlayerModel.Instance.FindPlayer(openId);
        var tran = GetHeadTran(info);
        if (tran == null)
            return;
        
        var sideTip = GetSmallGiftBase();
        sideTip.IsActive = true;
        sideTip.RefreshContent(info,tran,giftId,RecycleSide);
        _hashSet.Add(sideTip);
    }

    /// <summary> 获取一个玩家送小礼物的脚本 /// </summary>
    private PlayerGiftIconItem GetSmallGiftBase()
    {
        return giftTipQueue.Count > 0 ? giftTipQueue.Dequeue() : UIUtility.CreatePage<PlayerGiftIconItem>(uiitem_giftIconItem, Trans);
    }
    
    /// <summary>获取玩家的头像位置,如果头像位置为空,就不显示礼物图标/// </summary>
    private Transform GetHeadTran(PlayerInfo info)
    {
        Transform headTran;
        if (info.campType == CampType.蓝)
            headTran = BlueCampController.Instance.GetHeadTransform(info.SitIndex);
        else
            headTran = RedCampController.Instance.GetHeadTransform(info.SitIndex);

        return headTran;
    }

    /// <summary> 相机旋转,刷新3d物体坐标 /// </summary>
    private void RefreshGiftIconPoint(bool cameraRotation)
    {
        List<PlayerGiftIconItem> items = _hashSet.ToList();
        for (int i = items.Count - 1; i >= 0; i--)
            items[i].RefreshTargetPoint(cameraRotation);  
    }
    
    private void RecycleSide(PlayerGiftIconItem item)
    {
        item.IsActive = false;
        giftTipQueue.Enqueue(item);
        _hashSet.Remove(item);
    }

    public override void Close()
    {
        base.Close();
        foreach (var VARIABLE in _hashSet)
        {
            giftTipQueue.Enqueue(VARIABLE);
        }
        _hashSet.Clear();
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<string, string, long>(UIEvent.GiftSideView_gift_tip, RefreshGiftTip);
        EventMgr.AddEventListener<bool>(BattleEvent.Battle_Camera_Rotation, RefreshGiftIconPoint);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<string, string, long>(UIEvent.GiftSideView_gift_tip, RefreshGiftTip);
        EventMgr.RemoveEventListener<bool>(BattleEvent.Battle_Camera_Rotation, RefreshGiftIconPoint);
    }
}