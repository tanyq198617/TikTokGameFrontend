using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestTopZhanliPage : AItemPageBase
{

    private TextMeshProUGUI lb_zhanli;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        lb_zhanli = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_zhanli");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
