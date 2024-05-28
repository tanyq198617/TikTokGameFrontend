using System;
using HotUpdateScripts;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

//玩家点赞Item
public class PlayerLikeTipItem : PlayerTipItemBase
{
    private Image Image_tip;
 
    private static int seed;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        Image_tip = UIUtility.GetComponent<Image>(m_gameobj, "Image_tip");
    }

    public override void RefreshContent(PlayerInfo _info, Action<PlayerTipItemBase> _action)
    {
        base.RefreshContent(_info, _action);
        info = _info;
        onTweenCompleteAction += _action;
        
        int index = MathUtility.GetRandomNum(TLiketipDataManager.Instance.GetItemCount(), Guid.NewGuid().GetHashCode() + ++seed);

        TLiketipData data = TLiketipDataManager.Instance.GetAllItem()[index];
        if (data != null)
            UIUtility.Safe_UGUI(ref Image_tip, SpriteMgr.Instance.LoadSpriteFromMainView(data.spriteName));
        Image_tip.SetNativeSize();
        
        UIUtility.Safe_UGUI(ref tx_name, info.nickname);
        headItem.SetPlayerHead(info);
        switch (info.campType)
        {
            case CampType.红:
                PlayRedTween();
                break;
            case CampType.蓝:
                PlayBlueTween();
                break;
            default:
                break;
        }
    }
}