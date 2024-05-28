using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightCenterBoxPage : AItemPageBase
{

    private FightCenterBoxBox1Page box1Page;
    private FightCenterBoxBox2Page box2Page;
    private FightCenterBoxBox3Page box3Page;
    private FightCenterBoxBox4Page box4Page;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        box1Page = UIUtility.CreatePageNoClone<FightCenterBoxBox1Page>(RectTrans, "uiitem_box1");
        box2Page = UIUtility.CreatePageNoClone<FightCenterBoxBox2Page>(RectTrans, "uiitem_box2");
        box3Page = UIUtility.CreatePageNoClone<FightCenterBoxBox3Page>(RectTrans, "uiitem_box3");
        box4Page = UIUtility.CreatePageNoClone<FightCenterBoxBox4Page>(RectTrans, "uiitem_box4");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
