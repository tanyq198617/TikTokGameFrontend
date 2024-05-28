using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家坐下提示基类
/// </summary>
public class PlayerSideTip : AItemPageBase
{
    public event Action<PlayerSideTip> onTweenComplete = null;
    private Sequence tween;
    private Vector3 redStartPoint = new Vector3(-300, -272, 0);
    private Vector3 redEndPoint = new Vector3(146, -272, 0);
    private Vector3 blueStartPoint = new Vector3(-300, -272, 0);
    private Vector3 blueEndPoint = new Vector3(146, -272, 0);

    private TextMeshProUGUI tx_name;
    private TextMeshProUGUI tx_shuizhidao;

    private GameObject object_bluebjImage;
    private GameObject object_redbjImage;
    private PlayerHeadItem headItem;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_name = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_name");
        tx_shuizhidao = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_shuizhidao");
        object_bluebjImage = UIUtility.Control("object_bluebjImage", m_gameobj);
        object_redbjImage = UIUtility.Control("object_redbjImage", m_gameobj);
        headItem = UIUtility.CreateItemNoClone<PlayerHeadItem>(RectTrans,"uiitem_playerHeadObject");
    }

    public override void Refresh()
    {
        base.Refresh();
        Trans.SetAsLastSibling();
    }

    public override void Close()
    {
        ClearTween();
        onTweenComplete = null;
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

    public void RefreshContent(PlayerInfo info, Action<PlayerSideTip> action)
    {
        this.onTweenComplete += action;
        UIUtility.Safe_UGUI(ref tx_name, info.nickname);
        headItem.SetPlayerHead(info);
        switch (info.campType)
        {
            case CampType.红:
                object_redbjImage.SetActiveEX(true);
                object_bluebjImage.SetActiveEX(false);
                UIUtility.Safe_UGUI(ref tx_shuizhidao, "加入橙方");
                PlayRedTween();
                break;
            case CampType.蓝:
                object_bluebjImage.SetActiveEX(true);
                object_redbjImage.SetActiveEX(false);
                UIUtility.Safe_UGUI(ref tx_shuizhidao, "加入蓝方");
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