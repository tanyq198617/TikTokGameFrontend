using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestCenterBoxBox4Page : AItemPageBase
{

    private TextMeshProUGUI lb_none;
    private TextMeshProUGUI lb_title;
    private TextMeshProUGUI lb_name;

    private Image img_icon;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        lb_none = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_none");
        lb_title = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_title");
        lb_name = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_name");

        img_icon = UIUtility.GetComponent<Image>(RectTrans, "img_icon");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
