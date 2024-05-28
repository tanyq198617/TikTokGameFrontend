using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightTopPage : AItemPageBase
{

    private FightTopResourcePage resourcePage;
    private FightTopZhanliPage zhanliPage;
    private FightTopMissionPage missionPage;
    private FightTopButtonsPage buttonsPage;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        resourcePage = UIUtility.CreatePageNoClone<FightTopResourcePage>(RectTrans, "uiitem_resource");
        zhanliPage = UIUtility.CreatePageNoClone<FightTopZhanliPage>(RectTrans, "uiitem_zhanli");
        missionPage = UIUtility.CreatePageNoClone<FightTopMissionPage>(RectTrans, "uiitem_mission");
        buttonsPage = UIUtility.CreatePageNoClone<FightTopButtonsPage>(RectTrans, "uiitem_buttons");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
