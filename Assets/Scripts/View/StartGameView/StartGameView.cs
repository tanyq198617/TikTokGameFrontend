using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.StartGameView)]
public class StartGameView : APanelBase
{
    public static StartGameView Instance { get { return Singleton<StartGameView>.Instance; } }

    public StartGameView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }

    private StartGameBottomPage bottomPage;
    private StartGameCenterPage centerPage;

    public override void Init()
    {
        base.Init();

        bottomPage = UIUtility.CreatePageNoClone<StartGameBottomPage>(Trans, "Bottom");
        centerPage = UIUtility.CreatePageNoClone<StartGameCenterPage>(Trans, "Center");
    }

    public override void Refresh()
    {
        base.Refresh();
        bottomPage.IsActive = true;
        centerPage.IsActive = true;
    }

    public override void CloseUI()
    {
        base.CloseUI();
        bottomPage.IsActive = false;
        centerPage.IsActive = false;
    }
}
