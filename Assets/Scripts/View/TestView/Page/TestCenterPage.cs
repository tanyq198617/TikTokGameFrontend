using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestCenterPage : AItemPageBase
{

    private TestCenterLevelPage levelPage;
    private TestCenterBoxPage boxPage;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        levelPage = UIUtility.CreatePageNoClone<TestCenterLevelPage>(RectTrans, "uiitem_level");
        boxPage = UIUtility.CreatePageNoClone<TestCenterBoxPage>(RectTrans, "uiitem_box");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
