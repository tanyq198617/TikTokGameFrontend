using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestLeftResourceCoinPage : AItemPageBase
{

    private TextMeshProUGUI lb_di;
    private TextMeshProUGUI lb_value;

    private GameObject btn_add;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        lb_di = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_di");
        lb_value = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_value");

        btn_add = UIUtility.BindClickEvent(RectTrans, "btn_add", OnClick);

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_add)) 
        {
        }

    }
}
