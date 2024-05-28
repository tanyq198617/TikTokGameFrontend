using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestTopPage : AItemPageBase
{

    private TestTopResourcePage resourcePage;
    private TestTopZhanliPage zhanliPage;
    private TestTopMissionPage missionPage;
    private TestTopButtonsPage buttonsPage;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        resourcePage = UIUtility.CreatePageNoClone<TestTopResourcePage>(RectTrans, "uiitem_resource");
        zhanliPage = UIUtility.CreatePageNoClone<TestTopZhanliPage>(RectTrans, "uiitem_zhanli");
        missionPage = UIUtility.CreatePageNoClone<TestTopMissionPage>(RectTrans, "uiitem_mission");
        buttonsPage = UIUtility.CreatePageNoClone<TestTopButtonsPage>(RectTrans, "uiitem_buttons");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
