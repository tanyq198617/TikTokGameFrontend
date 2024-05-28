using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestCenterLevelPage : AItemPageBase
{

    private TextMeshProUGUI lb_title;
    private TextMeshProUGUI lb_desc;

    private GameObject btn_1;
    private GameObject btn_2;
    private GameObject btn_3;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        lb_title = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_title");
        lb_desc = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_desc");

        btn_1 = UIUtility.BindClickEvent(RectTrans, "btn_1", OnClick);
        btn_2 = UIUtility.BindClickEvent(RectTrans, "btn_2", OnClick);
        btn_3 = UIUtility.BindClickEvent(RectTrans, "btn_3", OnClick);

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_1)) 
        {
        }
        else if (obj.Equals(btn_2)) 
        {
        }
        else if (obj.Equals(btn_3)) 
        {
        }

    }
}
