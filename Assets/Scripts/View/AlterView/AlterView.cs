using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public enum AlterType
{
    OK,
    Cancel,
    Ok_Cancel,
    Ok_Cancel_Back,
}

[UIBind(UIPanelName.AlterView)]
public class AlterView : APanelBase
{
    public static AlterView Instance { get { return Singleton<AlterView>.Instance; } }

    public AlterView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.Other;
    }

    private GameObject btn_ok;
    private GameObject btn_cancel;
    private GameObject btn_close;
    private GameObject btn_locked;
    private GameObject line2;

    private TextMeshProUGUI lb_content;
    private TextMeshProUGUI lb_title;
    private TextMeshProUGUI lb_ok;
    private TextMeshProUGUI lb_cancel;

    private Action OkEvent, CancelEvent;
    private Action<bool> togOkEvent;

    public override void Init()
    {
        base.Init();

        lb_title = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_title");
        lb_content = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_content");
        lb_ok = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_ok");
        lb_cancel = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_cancel");
        line2 = UIUtility.Control("line2", m_gameobj);

        btn_locked = UIUtility.BindClickEvent(Trans, "btn_locked", OnClick);
        btn_ok = UIUtility.BindClickEvent(Trans, "btn_ok", OnClick);
        btn_cancel = UIUtility.BindClickEvent(Trans, "btn_cancel", OnClick);
        btn_close = UIUtility.BindClickEvent(Trans, "btn_close", OnClick);
    }

    public void ShowAlter(AlterType type, string msg, Action okEvent = null, Action cancalEvent = null, string tittle = "", string okName = "", string cancelName = "")
    {
        this.OkEvent = okEvent;
        this.CancelEvent = cancalEvent;

        if (string.IsNullOrEmpty(tittle))
            tittle = "提示";//LocalizationMgr.Instance.GetString("Common_Tips");

        if (string.IsNullOrEmpty(okName))
            okName = "确定";//LocalizationMgr.Instance.GetString("Common_Confirm");

        if (string.IsNullOrEmpty(cancelName))
            cancelName = "取消";//LocalizationMgr.Instance.GetString("Common_Cancel");

        if (!IsOpen)
        {
            UIMgr.Instance.ShowUI(UIPanelName.AlterView, openCall: (p) =>
            {
                RefreshContent(type, msg, tittle, okName, cancelName);
            });
        }
        else RefreshContent(type, msg, tittle, okName, cancelName);

    }

    public void ShowAlter(AlterType type, string msg, Action<bool> togOkEvent, Action cancalEvent = null, string tittle = "", string okName = "", string cancelName = "")
    {
        this.togOkEvent = togOkEvent;
        this.CancelEvent = cancalEvent;

        if (string.IsNullOrEmpty(tittle))
            tittle = "提示";//LocalizationMgr.Instance.GetString("Common_Tips");

        if (string.IsNullOrEmpty(okName))
            okName = "确定";//LocalizationMgr.Instance.GetString("Common_Confirm");

        if (string.IsNullOrEmpty(cancelName))
            cancelName = "取消";//LocalizationMgr.Instance.GetString("Common_Cancel");

        if (!IsOpen)
        {
            UIMgr.Instance.ShowUI(UIPanelName.AlterView, openCall: (p) =>
            {
                RefreshContent(type, msg, tittle, okName, cancelName);
            });
        }
        else RefreshContent(type, msg, tittle, okName, cancelName);
    }

    private void RefreshContent(AlterType type, string msg, string tittle, string okName, string cancelName)
    {
        UIUtility.Safe_UGUI(ref lb_title, tittle);
        UIUtility.Safe_UGUI(ref lb_content, msg);
        UIUtility.Safe_UGUI(ref lb_ok, okName);
        UIUtility.Safe_UGUI(ref lb_cancel, cancelName);

       // btn_ok.SetActiveEX(type != AlterType.Cancel);
       // btn_cancel.SetActiveEX(type != AlterType.OK);
       // btn_close?.SetActiveEX(type == AlterType.Ok_Cancel_Back);
       // line2.SetActiveEX(type != AlterType.OK);
    }

    private void OnOkEvent()
    {
        OkEvent?.Invoke();
        CloseUI();
    }

    private void OnCancelEvent()
    {
        CancelEvent?.Invoke();
        CloseUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        OkEvent = null;
        CancelEvent = null;
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);
        if (obj.Equals(btn_ok) || obj.Equals(btn_locked))
        {
            OnOkEvent();
        }
        else if (obj.Equals(btn_cancel))
        {
            OnCancelEvent();
        }
        else if (obj.Equals(btn_close))
        {
            OnCancelEvent();
        }
    }
}
