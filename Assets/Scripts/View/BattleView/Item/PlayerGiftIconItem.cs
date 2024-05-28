using System;
using DG.Tweening;
using HotUpdateScripts;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 玩家送礼物,礼物图标旋转缩小到玩家头像上item /// </summary>
public class PlayerGiftIconItem : AItemPageBase
{

    public event Action<PlayerGiftIconItem> onTweenComplete = null;
    
    private Image Image_giftIcon;
    private Transform transform_giftIcon;
    private WorldPointToUIPointComponent pointComponent;
    private Sequence tween;

    /// <summary> 玩家数据 /// </summary>
    private PlayerInfo info;
    
    //头像y偏移量,相机旋转
    private Vector3 headMaskYOffset;
    private Vector3 headMaskInitPoint;
    
    //相机坐标z轴过中线,视觉下台子会反转过来,重置头像的headmask y轴坐标
    private bool cameraRotation;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        Image_giftIcon = UIUtility.GetComponent<Image>(m_gameobj, "Image_giftIcon");
        transform_giftIcon = Image_giftIcon.transform;
        pointComponent ??= Trans.GetOrAddComponent<WorldPointToUIPointComponent>();
        headMaskYOffset = new Vector3(0, 150, 0);
        headMaskInitPoint =  new Vector3(0, -20, 0);
        cameraRotation = false;
    }

    public override void Refresh()
    {
        base.Refresh();
        cameraRotation = CameraMgr.Instance.cameraController != null && CameraMgr.Instance.cameraController.greaterZhanOne;
    }

    /// <summary> 设置玩家参数 /// </summary>
    public void RefreshContent(PlayerInfo _info, Transform _headTran, string _giftId, Action<PlayerGiftIconItem> _action)
    {
        onTweenComplete += _action;
        this.info = _info;
        SetHeadMaskOffset();
        
        var dataCfg = TOperateDataManager.Instance.GetItem(int.Parse(_giftId));

        if (dataCfg.giftImageName != null)
        {
            UIUtility.Safe_UGUI(ref Image_giftIcon, SpriteMgr.Instance.LoadSpriteFromCommon(dataCfg.giftImageName));
            Image_giftIcon.SetNativeSize();
        }

        pointComponent.enabled = true;
        pointComponent.SetTargetTransform(_headTran);
        PlayTween();
    }
    
    /// <summary> 播放tween动画 /// </summary>
    private void PlayTween()
    {
        ClearTween();
        transform_giftIcon.localScale = Vector3.one * 4;
        tween =  DOTween.Sequence();
        tween.Append(transform_giftIcon.DOScale(Vector3.zero, TGlobalDataManager.GiftIconTweenTime));
        tween.Join(transform_giftIcon.DORotate(new Vector3(0, 0, 360), TGlobalDataManager.GiftIconTweenTime, RotateMode.FastBeyond360));
        tween.OnComplete(TweenBack);
        tween.Play();
    }

    /// <summary> 重置头像headMask y轴 /// </summary>
    private void SetHeadMaskOffset()
    {
        if (info == null)
            return;
        if (info.SitIndex > 3)
        {
            transform_giftIcon.localPosition = Vector3.zero;
            return;
        }
        
        if (info.campType == CampType.蓝)
            transform_giftIcon.localPosition = cameraRotation ? headMaskInitPoint : headMaskYOffset;
        else
            transform_giftIcon.localPosition = cameraRotation ? headMaskYOffset : headMaskInitPoint;
    }

    public void RefreshTargetPoint(bool _cameraRotation)
    {
        cameraRotation = _cameraRotation;
        SetHeadMaskOffset();
        pointComponent?.RefreshTargetPoint();
    }
    
    private void TweenBack()
    {
        onTweenComplete?.Invoke(this);
        onTweenComplete = null;
    }
    
    private void ClearTween()
    {
        tween?.Kill(false);
        tween = null;
    }

    public override void Close()
    {
        base.Close();
        ClearTween();
        pointComponent.enabled = false;
        info = null;
    }
}