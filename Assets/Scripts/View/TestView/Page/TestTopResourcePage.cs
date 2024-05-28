using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestTopResourcePage : AItemPageBase
{

    private TestTopResourceLevelPage levelPage;
    private TestTopResourceBaoziPage baoziPage;
    private TestTopResourceCoinPage coinPage;
    private TestTopResourceYuPage yuPage;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        levelPage = UIUtility.CreatePageNoClone<TestTopResourceLevelPage>(RectTrans, "uiitem_level");
        baoziPage = UIUtility.CreatePageNoClone<TestTopResourceBaoziPage>(RectTrans, "uiitem_baozi");
        coinPage = UIUtility.CreatePageNoClone<TestTopResourceCoinPage>(RectTrans, "uiitem_coin");
        yuPage = UIUtility.CreatePageNoClone<TestTopResourceYuPage>(RectTrans, "uiitem_yu");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
