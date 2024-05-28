using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestLeftResourcePage : AItemPageBase
{

    private TestLeftResourceLevelPage levelPage;
    private TestLeftResourceBaoziPage baoziPage;
    private TestLeftResourceCoinPage coinPage;
    private TestLeftResourceYuPage yuPage;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        levelPage = UIUtility.CreatePageNoClone<TestLeftResourceLevelPage>(RectTrans, "uiitem_level");
        baoziPage = UIUtility.CreatePageNoClone<TestLeftResourceBaoziPage>(RectTrans, "uiitem_baozi");
        coinPage = UIUtility.CreatePageNoClone<TestLeftResourceCoinPage>(RectTrans, "uiitem_coin");
        yuPage = UIUtility.CreatePageNoClone<TestLeftResourceYuPage>(RectTrans, "uiitem_yu");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
