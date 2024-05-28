using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.BuffView)]
public class BuffView : APanelBase
{
    public static BuffView Instance { get { return Singleton<BuffView>.Instance; } }

    public BuffView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }    

    private BuffCenterPage centerPage;

    public override void Init()
    {
        base.Init();

        centerPage = UIUtility.CreatePageNoClone<BuffCenterPage>(Trans, "Center");

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
