using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartGameBottomPage : AItemPageBase
{

    private GameObject btn_enter;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        btn_enter = UIUtility.BindClickEvent(RectTrans, "btn_enter", OnClick);

    }

    private void OnEnterGame()
    {
        //没有网络时，重新连接
        if (!NetMgr.Instance.IsConnected)
        {
            EventMgr.Dispatch(ServerEvent.S2C_RetLogin_Fail);
            return;
        }

        //切换到主场景
        GameStageMachine.ChangeState<CityStage>();
    }


    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_enter))
        {
            OnEnterGame();
        }
    }
}
