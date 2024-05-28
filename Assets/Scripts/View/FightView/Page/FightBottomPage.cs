using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightBottomPage : AItemPageBase
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
            //PopView.Instance.IsDevelop("战斗");
            //GameStageMachine.ChangeState<BattleStage>();
            BattleGlobal.Instance.OnEnter<GeneralBattleControl>(10301);
        }
    }
}
