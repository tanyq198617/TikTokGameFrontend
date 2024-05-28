using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestCenterBoxPage : AItemPageBase
{

    private TestCenterBoxBox1Page box1Page;
    private TestCenterBoxBox2Page box2Page;
    private TestCenterBoxBox3Page box3Page;
    private TestCenterBoxBox4Page box4Page;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        box1Page = UIUtility.CreatePageNoClone<TestCenterBoxBox1Page>(RectTrans, "uiitem_box1");
        box2Page = UIUtility.CreatePageNoClone<TestCenterBoxBox2Page>(RectTrans, "uiitem_box2");
        box3Page = UIUtility.CreatePageNoClone<TestCenterBoxBox3Page>(RectTrans, "uiitem_box3");
        box4Page = UIUtility.CreatePageNoClone<TestCenterBoxBox4Page>(RectTrans, "uiitem_box4");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
