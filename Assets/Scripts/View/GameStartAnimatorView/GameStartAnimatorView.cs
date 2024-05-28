using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.GameStartAnimatorView)]
public class GameStartAnimatorView : APanelBase
{
    public static GameStartAnimatorView Instance { get { return Singleton<GameStartAnimatorView>.Instance; } }

    private Sequence tween;

    private Action onComplete;

    private GameObject map1StartAnimator;
    private GameObject map2StartAnimator;
    
    public GameStartAnimatorView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.Mask;
    }

    public override void Init()
    {
        base.Init();
        map1StartAnimator = UIUtility.Control("E2D_UI_kaishi", m_gameobj);
        map2StartAnimator = UIUtility.Control("E2D_UI_kaishi_2", m_gameobj);
    }

    public override void Refresh()
    {
        base.Refresh();
        if (LevelGlobal.Instance.DataBase.RegionId == 2)
        {
            map1StartAnimator.SetActiveEX(true);
            map2StartAnimator.SetActiveEX(false);
        }
        else
        {
            map2StartAnimator.SetActiveEX(true);
            map1StartAnimator.SetActiveEX(false);
        }
    }

    public void OpenUI(Action callback = null)
    {
        this.onComplete = callback;
        if (!IsOpen)
        {
            UIMgr.Instance.ShowUI(UIPanelName.GameStartAnimatorView, (p) =>
            {
                tween = DOTween.Sequence();
                tween.AppendInterval(2.3f);
                tween.OnComplete(CloseUI);
            });
        }
    }

    public override void CloseUI()
    {
        base.CloseUI();
        tween?.Kill(false);
        tween = null;
        onComplete = null;
    }
}
