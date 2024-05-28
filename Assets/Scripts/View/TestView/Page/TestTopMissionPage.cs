using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestTopMissionPage : AItemPageBase
{

    private TextMeshProUGUI lb_zhanli;
    private TextMeshProUGUI lb_info;
    private TextMeshProUGUI lb_lv;
    private TextMeshProUGUI lb_exp;

    private GameObject btn_go;
    private GameObject btn_get;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        lb_zhanli = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_zhanli");
        lb_info = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_info");
        lb_lv = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_lv");
        lb_exp = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_exp");

        btn_go = UIUtility.BindClickEvent(RectTrans, "btn_go", OnClick);
        btn_get = UIUtility.BindClickEvent(RectTrans, "btn_get", OnClick);

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_go)) 
        {
        }
        else if (obj.Equals(btn_get)) 
        {
        }

    }
}
