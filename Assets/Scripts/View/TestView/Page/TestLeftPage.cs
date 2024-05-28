using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestLeftPage : AItemPageBase
{

    private TestLeftResourcePage resourcePage;
    private TestLeftZhanliPage zhanliPage;
    private TestLeftMissionPage missionPage;
    private TestLeftButtonsPage buttonsPage;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        resourcePage = UIUtility.CreatePageNoClone<TestLeftResourcePage>(RectTrans, "uiitem_resource");
        zhanliPage = UIUtility.CreatePageNoClone<TestLeftZhanliPage>(RectTrans, "uiitem_zhanli");
        missionPage = UIUtility.CreatePageNoClone<TestLeftMissionPage>(RectTrans, "uiitem_mission");
        buttonsPage = UIUtility.CreatePageNoClone<TestLeftButtonsPage>(RectTrans, "uiitem_buttons");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
