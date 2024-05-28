using System;
using DG.Tweening;
using HotUpdateScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBigGiftTip : AItemPageBase
{
    public Action<PlayerBigGiftTip> onTweenComplete;
    private Sequence tween;
    
    private TextMeshProUGUI tx_tip;
    private Image tip_image;
    private PlayerHeadItem headItem;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_tip = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_tip");
        tip_image = UIUtility.GetComponent<Image>(m_gameobj, "tip_image");
        headItem = UIUtility.CreateItemNoClone<PlayerHeadItem>(RectTrans,"uiitem_playerHeadObject");
    }

    public override void Close()
    {
        ClearTween();
        onTweenComplete = null;
        headItem.IsActive = false;
        base.Close();
    }

    private void ClearTween()
    {
        tween?.Kill(false);
        tween = null;
    }

    public override void Refresh()
    {
        base.Refresh();
        Trans.SetAsLastSibling();
    }

    public void RefreshContent(string openId, string giftId, long giftCount, Action<PlayerBigGiftTip> action)
    {
        this.onTweenComplete += action;
        PlayerInfo info = PlayerModel.Instance.FindPlayer(openId);
        TOperateData dataCfg = TOperateDataManager.Instance.GetItem(int.Parse(giftId));
        
        headItem.SetPlayerHead(info);
        
        string tipStr = dataCfg.uiXIanShiCount >= 0
            ? $"{dataCfg.uiXIanShi}x{dataCfg.uiXIanShiCount * giftCount}"
            : dataCfg.uiXIanShi;
        UIUtility.Safe_UGUI(ref tx_tip,
            $"<color=#FFAA00>{info.nickname}召唤{tipStr}");
        
        string tipImageName =
            info.campType == CampType.红 ? $"red_{dataCfg.tipImageName}" : $"blue_{dataCfg.tipImageName}";
        UIUtility.Safe_UGUI(ref tip_image, SpriteMgr.Instance.LoadSpriteFromMainView(tipImageName));
        tip_image.SetNativeSize();
        
        PlayTween();
    }

    /// <summary> 播放tween动画,用作延时回收 /// </summary>
    public void PlayTween()
    {
        ClearTween();
        m_gameobj.transform.localPosition = Vector3.zero;
        tween = DOTween.Sequence();
        tween.AppendInterval(1);
        tween.OnComplete(OnTweenComplete);
    }

    private void OnTweenComplete()
    {
        ClearTween();
        if (onTweenComplete != null)
            onTweenComplete(this);
    }
}