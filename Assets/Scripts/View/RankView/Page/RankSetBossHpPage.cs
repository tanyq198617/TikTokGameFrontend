using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankSetBossHpPage : AItemPageBase
{
    TextMeshProUGUI hpText0, hpText1, hpText2;
    
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        /*string hpCfgStr = TGlobalDataManager.Instance.GetByKey(TGlobal.MainBossHp);
        string[] ss = hpCfgStr.Split('/');

        hpText0.text = ss[0] + "耐力";
        hpText1.text = ss[1] + "耐力";
        hpText2.text = ss[2] + "耐力";*/
        
        for (int i = 1; i < 4; i++)
        {
            int index = i;
            UIUtility.BindToggleClick<int>(RectTrans, "toggle" + index, OnToggleClick, index);
        }
    }

    private void OnToggleClick(bool agree, int index)
    {
        if (agree)
            Debuger.LogError("选择游戏模式:" + index.ToString());
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);
    }
}