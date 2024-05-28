using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightTopResourcePage : AItemPageBase
{

    private FightTopResourceLevelPage levelPage;
    private FightTopResourceBaoziPage baoziPage;
    private FightTopResourceCoinPage coinPage;
    private FightTopResourceYuPage yuPage;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        levelPage = UIUtility.CreatePageNoClone<FightTopResourceLevelPage>(RectTrans, "uiitem_level");
        baoziPage = UIUtility.CreatePageNoClone<FightTopResourceBaoziPage>(RectTrans, "uiitem_baozi");
        coinPage = UIUtility.CreatePageNoClone<FightTopResourceCoinPage>(RectTrans, "uiitem_coin");
        yuPage = UIUtility.CreatePageNoClone<FightTopResourceYuPage>(RectTrans, "uiitem_yu");

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);


    }
}
