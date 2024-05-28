using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.SettingView)]
public class SettingView : APanelBase
{
    public static SettingView Instance { get { return Singleton<SettingView>.Instance; } }

    public SettingView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }    

    private SettingBottomPage bottomPage;

    public override void Init()
    {
        base.Init();

        bottomPage = UIUtility.CreatePageNoClone<SettingBottomPage>(Trans, "Bottom");

    }

    public override void Refresh()
    {
        base.Refresh();
        bottomPage.IsActive = true;
    }

    public override void CloseUI()
    {
        base.CloseUI();
        bottomPage.IsActive = false;
    }
}
