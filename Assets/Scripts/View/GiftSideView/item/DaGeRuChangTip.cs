using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DaGeRuChangTip : AItemPageBase
{
    public event Action<DaGeRuChangTip> onTweenComplete;
    private Sequence tween;

    private TextMeshProUGUI tx_playerName;
    private TextMeshProUGUI tx_worldRank;

    public NoPlayDaGeRuChangStruct ruChangStruct;
    private PlayerHeadItem headItem;

    public int daGetype;

    private float tweenTime = 2;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        daGetype = 1;
        tx_playerName = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_playerName");
        tx_worldRank = UIUtility.GetComponent<TextMeshProUGUI>(m_gameobj, "tx_worldRank");
        headItem = UIUtility.CreateItemNoClone<PlayerHeadItem>(RectTrans, "uiitem_playerHeadObject");
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
        headItem.IsActive = false;
        base.Close();
    }
    
    private void ClearTween()
    {
        tween?.Kill(false);
        tween = null;
    }

    public void RefreshContent(NoPlayDaGeRuChangStruct info, Action<DaGeRuChangTip> action)
    {
        PlayAudio();
        this.ruChangStruct = info;
        this.onTweenComplete += action;
        UIUtility.Safe_UGUI(ref tx_playerName, info.playerInfo.nickname);
        UIUtility.Safe_UGUI(ref tx_worldRank, $"世界排名:{info.playerInfo.world_rank}");
        headItem.SetPlayerHead(info.playerInfo);
        PlayTween();
    }

    /// <summary> 播放tween动画,用作延时回收 /// </summary>
    public void PlayTween()
    {
        ClearTween();
        Trans.localScale = Vector3.zero;
        tween = DOTween.Sequence();
        tween.Append(Trans.DOScale(Vector3.one, 0.3f));
        tween.AppendInterval(tweenTime);
        tween.Append(Trans.DOScale(Vector3.zero, 0.1f));
        tween.OnComplete(OnTweenComplete);
        tween.Play();
    }

    private void OnTweenComplete()
    {
        ClearTween();
        if (onTweenComplete != null)
            onTweenComplete(this);
    }

    private void PlayAudio()
    {
        switch (daGetype)
        {
            case 1:
                AudioMgr.Instance.PlaySoundName(8);
                break;
            case 2:
                AudioMgr.Instance.PlaySoundName(9);
                break;
            case 3:
                AudioMgr.Instance.PlaySoundName(10);
                break;
            default:
                break;
        }
    }
}