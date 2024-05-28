using System.Collections.Generic;
using UnityEngine;

public class GiftSideCenterPageBase : AItemPageBase
{
    protected GameObject uiitem_Tip;
    private Transform redTweenStartTran;
    private Transform redTweenEndTran;
    private Transform blueTweenStartTran;
    private Transform blueTweenEndTran;

    /// <summary> 回收的队列 /// </summary>
    protected Queue<PlayerTipItemBase> tipQueue;
    
    /// <summary> 正在播放的队列 /// </summary>
    protected HashSet<PlayerTipItemBase> _hashSet;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

      
        redTweenStartTran = UIUtility.Control("redTweenStartTran", Trans);
        redTweenEndTran = UIUtility.Control("redTweenEndTran", Trans);
        blueTweenStartTran = UIUtility.Control("blueTweenStartTran", Trans);
        blueTweenEndTran = UIUtility.Control("blueTweenEndTran", Trans);
        tipQueue = new Queue<PlayerTipItemBase>();
        _hashSet = new HashSet<PlayerTipItemBase>();
    }


    /// <summary> 获取一个玩家送小礼物的脚本 /// </summary>
    protected T GetSmallGiftBase<T>() where T : PlayerTipItemBase, new()
    {
        PlayerTipItemBase tip = null;
        if (tipQueue.Count > 0)
        {
            tip = tipQueue.Dequeue();
        }
        else
        {
            tip = UIUtility.CreatePage<T>(uiitem_Tip, Trans);
        }

        tip.IsActive = true;
        tip.SetTweenPoint(redTweenStartTran.localPosition, redTweenEndTran.localPosition, blueTweenStartTran.localPosition,
            blueTweenEndTran.localPosition);
        return tip as T;
    }

    public override void Close()
    {
        base.Close();
        foreach (var item in _hashSet)
        {
            item.IsActive = false;
            tipQueue.Enqueue(item);
        }

        _hashSet.Clear();
    }
}