using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.GiftSideView)]
public class GiftSideView : APanelBase
{
    public static GiftSideView Instance
    {
        get { return Singleton<GiftSideView>.Instance; }
    }

    public GiftSideView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.Two;
    }

    private GiftSideCenterPage centerPage;

    public override void Init()
    {
        base.Init();

        centerPage = UIUtility.CreatePageNoClone<GiftSideCenterPage>(Trans, "Center");
    }

    public override void Refresh()
    {
        base.Refresh();
        centerPage.IsActive = true;
    }

    public override void CloseUI()
    {
        base.CloseUI();
        centerPage.IsActive = false;
    }
}