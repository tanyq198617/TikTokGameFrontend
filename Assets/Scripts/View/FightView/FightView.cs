using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[UIBind(UIPanelName.FightView)]
public class FightView : APanelBase
{
    public static FightView Instance { get { return Singleton<FightView>.Instance; } }

    public FightView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }    

    private FightCenterPage centerPage;
    private FightTopPage topPage;
    private FightBottomPage bottomPage;

    public override void Init()
    {
        base.Init();

        centerPage = UIUtility.CreatePageNoClone<FightCenterPage>(Trans, "Center");
        topPage = UIUtility.CreatePageNoClone<FightTopPage>(Trans, "Top");
        bottomPage = UIUtility.CreatePageNoClone<FightBottomPage>(Trans, "Bottom");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
