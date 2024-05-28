using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightTopResourceLevelPage : AItemPageBase
{

    private TextMeshProUGUI lb_di;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        lb_di = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "lb_di");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
