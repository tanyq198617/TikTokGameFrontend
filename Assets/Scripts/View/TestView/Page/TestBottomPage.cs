using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestBottomPage : AItemPageBase
{

    private GameObject btn_enter;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        btn_enter = UIUtility.BindClickEvent(RectTrans, "btn_enter", OnClick);

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_enter)) 
        {
        }

    }
}
