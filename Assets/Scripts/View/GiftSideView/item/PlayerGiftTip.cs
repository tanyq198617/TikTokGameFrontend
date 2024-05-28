using System;
using DG.Tweening;
using HotUpdateScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGiftTip : AItemPageBase
{
    public event Action<PlayerGiftTip> onTweenComplete = null;
    private Sequence tween;
    private Vector3 redStartPoint = new Vector3(-300, -272, 0);
    private Vector3 redEndPoint = new Vector3(146, -272, 0);
    private Vector3 blueStartPoint = new Vector3(-300, -272, 0);
    private Vector3 blueEndPoint = new Vector3(146, -272, 0);


    private TextMeshProUGUI tx_name;
    private TextMeshProUGUI tx_gift;

    private GameObject object_bluebjImage;
    private GameObject object_redbjImage;
    private PlayerHeadItem headItem;

    private Image image_gift;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_name = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_name");
        tx_gift = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_gift");
        image_gift = UIUtility.GetComponent<Image>(m_gameobj, "image_gift");
        object_bluebjImage = UIUtility.Control("object_bluebjImage", m_gameobj);
        object_redbjImage = UIUtility.Control("object_redbjImage", m_gameobj);
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

    public void SetTweenPoint(Vector3 redStartPoint, Vector3 redEndPoint, Vector3 blueStartPoint, Vector3 blueEndPoint)
    {
        this.redStartPoint = redStartPoint;
        this.redEndPoint = redEndPoint;
        this.blueStartPoint = blueStartPoint;
        this.blueEndPoint = blueEndPoint;
    }

    public override void Refresh()
    {
        base.Refresh();
        Trans.SetAsLastSibling();
    }

    public void RefreshContent(string openId, string giftId, long giftCount, Action<PlayerGiftTip> action)
    {
        AudioMgr.Instance.PlaySoundName(6);
        this.onTweenComplete += action;
        PlayerInfo info = PlayerModel.Instance.FindPlayer(openId);
        TOperateData dataCfg = TOperateDataManager.Instance.GetItem(int.Parse(giftId));

        if (dataCfg.giftImageName != null)
        {
            UIUtility.Safe_UGUI(ref image_gift, SpriteMgr.Instance.LoadSpriteFromCommon(dataCfg.giftImageName));
            image_gift.SetNativeSize();
        }
        
        UIUtility.Safe_UGUI(ref tx_name, info.nickname);
        
        headItem.SetPlayerHead(info);
        
        if (dataCfg.IsNotNull())
            UIUtility.Safe_UGUI(ref tx_gift, $"x{giftCount}");
        else
            UIUtility.Safe_UGUI(ref tx_gift, "");

        switch (info.campType)
        {
            case CampType.红:
                object_redbjImage.SetActiveEX(true);
                object_bluebjImage.SetActiveEX(false);
                PlayRedTween();
                break;
            case CampType.蓝:
                object_bluebjImage.SetActiveEX(true);
                object_redbjImage.SetActiveEX(false);
                PlayBlueTween();
                break;
            default:
                break;
        }
    }

    /// <summary> 播放蓝方的tween动画 /// </summary>
    public void PlayBlueTween()
    {
        ClearTween();
        m_gameobj.transform.localPosition = blueStartPoint;
        tween = DOTween.Sequence();
        tween.Append(Trans.DOLocalMove(blueEndPoint, 0.5f));
        tween.AppendInterval(1.5f);
        tween.OnComplete(OnTweenComplete);
    }

    /// <summary> 播放红方的tween动画 /// </summary>
    public void PlayRedTween()
    {
        ClearTween();
        m_gameobj.transform.localPosition = redStartPoint;
        tween = DOTween.Sequence();
        tween.Append(Trans.DOLocalMove(redEndPoint, 0.5f));
        tween.AppendInterval(1.5f);
        tween.OnComplete(OnTweenComplete);
    }

    private void OnTweenComplete()
    {
        ClearTween();
        if (onTweenComplete != null)
            onTweenComplete(this);
        object_bluebjImage.SetActiveEX(false);
        object_redbjImage.SetActiveEX(false);
    }
}