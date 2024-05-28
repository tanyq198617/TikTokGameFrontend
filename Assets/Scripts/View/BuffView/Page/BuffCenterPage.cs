using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffCenterPage : AItemPageBase
{

    private Text tx_redbuff;
    private Text tx_bluebuff;
    private GameObject redBuffObject;
    private GameObject blueBuffObject;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_redbuff = UIUtility.GetComponent<Text>(RectTrans, "tx_redbuff");
        tx_bluebuff = UIUtility.GetComponent<Text>(RectTrans, "tx_bluebuff");
        redBuffObject = UIUtility.Control("redBuffObject", m_gameobj);
        blueBuffObject = UIUtility.Control("blueBuffObject", m_gameobj);

    }

    public override void Refresh()
    {
        base.Refresh();
        redBuffObject.SetActiveEX(false);
        blueBuffObject.SetActiveEX(false);
    }

    private void SetBuffObject(CampType camp, BuffHandler handler)
    {
        switch (camp)
        {
            case CampType.红:
                SetRedBuff(handler);
                break;
            case CampType.蓝:
                SetBlueBuff(handler);
                break;
        }
    }

    /// <summary> 设置红方buff /// </summary>
    private void SetRedBuff(BuffHandler _buffHandler)
    {
        bool isActBuff = _buffHandler.Count() > 0;
        if (isActBuff)
        {
            redBuffObject.SetActiveEX(true);
            UIUtility.Safe_UGUI(ref tx_redbuff,_buffHandler.Count());
        }
        else
        {
            redBuffObject.SetActiveEX(false);
        }
    }

    /// <summary> 设置蓝方buff /// </summary>
    private void SetBlueBuff(BuffHandler _buffHandler)
    {
        bool isActBuff = _buffHandler.Count() > 0;
        if (isActBuff)
        {
            blueBuffObject.SetActiveEX(true);
            UIUtility.Safe_UGUI(ref tx_bluebuff,_buffHandler.Count());
        }
        else
        {
            blueBuffObject.SetActiveEX(false);
        }
    }
    
     public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<CampType, BuffHandler>(BattleEvent.Battle_BUFF_Changed, SetBuffObject);
        EventMgr.AddEventListener<CampType, BuffHandler>(BattleEvent.Battle_BUFF_Recycle, SetBuffObject);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<CampType, BuffHandler>(BattleEvent.Battle_BUFF_Changed, SetBuffObject);
        EventMgr.RemoveEventListener<CampType, BuffHandler>(BattleEvent.Battle_BUFF_Recycle, SetBuffObject);
    }
}
