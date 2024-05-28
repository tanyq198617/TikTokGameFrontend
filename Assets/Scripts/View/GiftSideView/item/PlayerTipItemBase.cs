using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary> 玩家送礼物,坐下,点赞的基类 /// </summary>
public class PlayerTipItemBase : AItemPageBase
{
    private Sequence tween;
    protected event Action<PlayerTipItemBase> onTweenCompleteAction = null;
    private Vector3 redStartPoint = new Vector3(-300, -272, 0);
    private Vector3 redEndPoint = new Vector3(146, -272, 0);
    private Vector3 blueStartPoint = new Vector3(-300, -272, 0);
    private Vector3 blueEndPoint = new Vector3(146, -272, 0);
    private GameObject object_bluebjImage;
    private GameObject object_redbjImage;
    
    protected TextMeshProUGUI tx_name;
    protected PlayerHeadItem headItem;

    public PlayerInfo info;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_name = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_name");
        object_bluebjImage = UIUtility.Control("object_bluebjImage", m_gameobj);
        object_redbjImage = UIUtility.Control("object_redbjImage", m_gameobj);
        headItem = UIUtility.CreateItemNoClone<PlayerHeadItem>(RectTrans,"uiitem_playerHeadObject");
    }
    
    
    public void SetTweenPoint(Vector3 redStartPoint, Vector3 redEndPoint, Vector3 blueStartPoint, Vector3 blueEndPoint)
    {
        this.redStartPoint = redStartPoint;
        this.redEndPoint = redEndPoint;
        this.blueStartPoint = blueStartPoint;
        this.blueEndPoint = blueEndPoint;
    }
    
    //刷新玩家点赞
    public virtual void RefreshContent(PlayerInfo _info, Action<PlayerTipItemBase> action)
    {
        
    }
    
    public override void Refresh()
    {
        base.Refresh();
        Trans.SetAsLastSibling();
    }
    public override void Close()
    {
        onTweenCompleteAction = null;
        object_bluebjImage.SetActiveEX(false);
        object_redbjImage.SetActiveEX(false);
        headItem.IsActive = false;
        ClearTween();
        base.Close();
    }
    
    private void ClearTween()
    {
        tween?.Kill(false);
        tween = null;
    }
    
    /// <summary> 播放蓝方的tween动画 /// </summary>
    public void PlayBlueTween()
    {
        object_bluebjImage.SetActiveEX(true);
        object_redbjImage.SetActiveEX(false);
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
        object_redbjImage.SetActiveEX(true);
        object_bluebjImage.SetActiveEX(false);
        ClearTween();
        m_gameobj.transform.localPosition = redStartPoint;
        tween = DOTween.Sequence();
        tween.Append(Trans.DOLocalMove(redEndPoint, 0.5f));
        tween.AppendInterval(1.5f);
        tween.OnComplete(OnTweenComplete);
        tween.Play();
    }

    private void OnTweenComplete()
    {
        if (onTweenCompleteAction != null)
            onTweenCompleteAction(this);
    }
}