using System;
using System.Net.Mime;
using HotUpdateScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>连胜点item/// </summary>
public class WinningPointTipItem : PlayerTipItemBase
{
    private TextMeshProUGUI tx_winningPoint;
    private Image image_tip;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        tx_winningPoint = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_winningPoint");
        image_tip = UIUtility.GetComponent<Image>(m_gameobj, "image_tip");
    }

    public void RefreshWinningPointContent(PlayerInfo _info,TWinningPointExpend expend, Action<PlayerTipItemBase> _action)
    {
        base.RefreshContent(_info, _action);
        info = _info;
        onTweenCompleteAction += _action;
        
        UIUtility.Safe_UGUI(ref tx_winningPoint, $"消耗胜点x{expend.consumeCount}");
        UIUtility.Safe_UGUI(ref tx_name, info.nickname);
        UIUtility.Safe_UGUI(ref image_tip, SpriteMgr.Instance.LoadSpriteFromMainView(expend.instructImageName));
        UIUtility.SetNativeSize(ref image_tip);
        
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