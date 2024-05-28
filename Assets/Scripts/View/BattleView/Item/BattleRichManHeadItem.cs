using System.Net.NetworkInformation;
using DG.Tweening;
using HotUpdateScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 设置红蓝方三个大哥的头像显示
/// </summary>
public class BattleRichManHeadItem : AItemPageBase
{
    private Transform headmask;
    private WorldPointToUIPointComponent pointComponent;
    private PlayerHeadItem headItem;

    private Tween scaleTween;
    public PlayerInfo info;

    private bool isPlayTween;

    //头像y偏移量,相机旋转
    private Vector3 headMaskYOffset;
    private Vector3 headMaskInitPoint;

    //头像y偏移量,相机旋转
    private Vector3 winningYOffset;
    private Vector3 winningInitPoint;

    private Transform uiitem_richManWinning;
    private TextMeshProUGUI tx_winning;

    //相机坐标z轴过中线,视觉下台子会反转过来,重置头像的headmask y轴坐标
    private bool cameraRotation;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        headMaskYOffset = new Vector3(0, 150, 0);
        headMaskInitPoint = new Vector3(0, -20, 0);
        winningYOffset = new Vector3(0, 200, 0);
        winningInitPoint = new Vector3(0, -77, 0);
        headmask = UIUtility.Control("Image_mask", Trans);
        pointComponent ??= m_gameobj.GetOrAddComponent<WorldPointToUIPointComponent>();
        headItem = UIUtility.CreateItemNoClone<PlayerHeadItem>(m_gameobj);
        uiitem_richManWinning = UIUtility.Control("uiitem_richManWinning", Trans);
        tx_winning = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_winning");
        cameraRotation = false;
        info = null;
    }

    public override void Refresh()
    {
        base.Refresh();
        cameraRotation = CameraMgr.Instance.cameraController != null &&
                         CameraMgr.Instance.cameraController.greaterZhanOne;
        isPlayTween = true;
    }

    /// <summary> 设置相机旋转状态 /// </summary>
    public void SetHeadMaskOffset(bool _cameraRotation)
    {
        cameraRotation = _cameraRotation;
        SetHeadMaskOffset();
    }

    /// <summary> 重置头像headMask y轴 /// </summary>
    private void SetHeadMaskOffset()
    {
        if (info == null)
            return;
        if (info.campType == CampType.蓝)
        {
            headmask.localPosition = cameraRotation ? headMaskInitPoint : headMaskYOffset;
            uiitem_richManWinning.localPosition = cameraRotation ? winningInitPoint : winningYOffset;
        }
        else
        {
            headmask.localPosition = cameraRotation ? headMaskYOffset : headMaskInitPoint;
            uiitem_richManWinning.localPosition = cameraRotation ? winningYOffset : winningInitPoint;
        }
    }

    /// <summary> 设置大哥的数据和排名数据 /// </summary>
    public void SetPlayerInfo(PlayerInfo _info, int rank)
    {
        this.info = _info;
        Transform rankTransform;
        if (_info.campType == CampType.蓝)
        {
            rankTransform = BlueCampController.Instance.GetRichManHeadTrans(rank);
        }
        else
        {
            rankTransform = RedCampController.Instance.GetRichManHeadTrans(rank);
        }

        if (rankTransform == null)
        {
            Debuger.Log($"添加大哥头像失败,排名:{rank},Url:{_info.avatar_url}");
            IsActive = false;
            return;
        }

        UIUtility.Safe_UGUI(ref tx_winning,$"{_info.win_combo}连");
        headItem.SetPlayerHead(_info);

        this.info = _info;
        pointComponent.enabled = true;
        pointComponent.SetTargetTransform(rankTransform);
        if (isPlayTween)
            PlayScaleTween();
        SetHeadMaskOffset();
    }

    private void PlayScaleTween()
    {
        isPlayTween = false;
        headmask.localScale = TGlobalDataManager.richManHeadStruct.initHeadScale;
        scaleTween = headmask.DOScale(Vector3.one, TGlobalDataManager.richManHeadStruct.scaleTime).SetAutoKill();
    }

    public override void Close()
    {
        base.Close();
        pointComponent.enabled = false;
        headItem.IsActive = false;
        scaleTween?.Kill(false);
        scaleTween = null;
        info = null;
        isPlayTween = false;
    }
}