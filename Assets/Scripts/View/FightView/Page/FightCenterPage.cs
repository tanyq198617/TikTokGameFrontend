using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightCenterPage : AItemPageBase
{

    private FightCenterLevelPage levelPage;
    private FightCenterBoxPage boxPage;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        levelPage = UIUtility.CreatePageNoClone<FightCenterLevelPage>(RectTrans, "uiitem_level");
        boxPage = UIUtility.CreatePageNoClone<FightCenterBoxPage>(RectTrans, "uiitem_box");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
