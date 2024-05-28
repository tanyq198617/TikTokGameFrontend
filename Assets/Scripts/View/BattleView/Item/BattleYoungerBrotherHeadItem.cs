using DG.Tweening;
using HotUpdateScripts;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 小弟头像
/// </summary>
public class BattleYoungerBrotherHeadItem : AItemPageBase
{
    private WorldPointToUIPointComponent _pointComponent;
    private PlayerHeadItem headItem;

    private PlayerHeadScaleTweenStruct scaleTweenStruct;
    private Tween scaleTween;
    private bool isPlayTween;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        _pointComponent ??= m_gameobj.GetOrAddComponent<WorldPointToUIPointComponent>();
        headItem = UIUtility.CreateItemNoClone<PlayerHeadItem>(m_gameobj);
        scaleTweenStruct = TGlobalDataManager.youngerBrotherHeadStruct;
    }

    public override void Refresh()
    {
        base.Refresh();
        isPlayTween = true;
    }

    public void SetPlayerInfo(int rank, PlayerInfo info)
    {
        Transform? tran;
        if (info.campType == CampType.蓝)
        {
            tran = BlueCampController.Instance.GetYoungerBrotherPoint(rank - 3);
        }
        else
        {
            tran = RedCampController.Instance.GetYoungerBrotherPoint(rank - 3);
        }

        if (tran == null)
        {
            Debuger.Log($"小弟头像没有追随的3d坐标,排名:{rank}");
            IsActive = false;
            return;
        }

        headItem.SetPlayerHead(info);
        
        _pointComponent.enabled = true;
        _pointComponent.SetTargetTransform(tran);

        if (isPlayTween)
        {
            PlayScaleTween();
        }
    }

    /// <summary>刷新头像跟3d坐标的坐标显示/// </summary>
    public void RefreshTargetPoint()
    {
        if( _pointComponent&&_pointComponent.enabled)
            _pointComponent.RefreshTargetPoint();
    }
    
    private void PlayScaleTween()
    {
        isPlayTween = false;
        Trans.localScale = scaleTweenStruct.initHeadScale;
        scaleTween = Trans.DOScale(Vector3.one, scaleTweenStruct.scaleTime).SetAutoKill();
    }

    public override void Close()
    {
        _pointComponent.enabled = false;
        headItem.IsActive = false;
        scaleTween.Kill(false);
        scaleTween = null;
        isPlayTween = false;
        base.Close();
    }
}