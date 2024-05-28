using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using System;
using TMPro;

internal class AlterView : APanelBase
{
    public static AlterView Instance { get { return Singleton<AlterView>.Instance; } }
    public AlterView() : base()
    {
        isFilm = true;
        m_IsLoadFromResources = true;
    }

    public enum AlterType
    {
        ok,
        ok_cancel
    }

    private GameObject btn_ok;
    private GameObject btn_cancel;

    private TextMeshProUGUI tx_title;
    private TextMeshProUGUI tx_content;
    private TextMeshProUGUI tx_ok;
    private TextMeshProUGUI tx_cancel;

    public Action<bool> onComplete { get; set; }

    public override void Init()
    {
        base.Init();

        tx_title = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_title");
        tx_content = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_content");
        tx_ok = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_ok");
        tx_cancel = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_cancel");

        btn_ok = UIUtility.BindClickEvent(Trans, "btn_ok", OnClick);
        btn_cancel = UIUtility.BindClickEvent(Trans, "btn_cancel", OnClick);
    }

    public void OkAndCancel(string title, string msg, string okStr = "", string cancelStr = "")
        => Show(title, msg, okStr, cancelStr);

    public void OkAndCancel(string msg, string okStr = "", string cancelStr = "") 
        => Show("", msg, okStr, cancelStr);

    public void Ok(string title, string msg, string okStr = "")
       => Show(title, msg, okStr, "");

    public void Ok(string msg, string okStr = "")
       => Show("", msg, okStr, "");


    private void Show(string title, string msg, string okStr, string cancelStr)
    {
        if (!IsOpen)
        {
            UIMgr.Instance.ShowUI(UIPanelName.AlterView, openCall: (p) =>
            {
                RefreshContent(title, msg, okStr, cancelStr);
            });
        }
        else
            RefreshContent(title, msg, okStr, cancelStr);
    }

    private void RefreshContent(string title, string msg, string okStr, string cancelStr)
    {
        UIUtility.Safe_UGUI(ref tx_title, GetTitle(title));
        UIUtility.Safe_UGUI(ref tx_content, msg);
        UIUtility.Safe_UGUI(ref tx_ok, GetOkName(okStr));
        UIUtility.Safe_UGUI(ref tx_cancel, GetCancelName(cancelStr));
    }

    private string GetTitle(string title) 
    {
        if (string.IsNullOrEmpty(title))
            return "提示";
        return title;
    }

    private string GetOkName(string title)
    {
        if (string.IsNullOrEmpty(title))
            return "确定";
        return title;
    }


    private string GetCancelName(string title)
    {
        if (string.IsNullOrEmpty(title))
            return "取消";
        return title;
    }

    private void OnOkClick()
    {
        CloseUI();
        onComplete?.Invoke(true);
    }

    private void OnCancelClick()
    {
        CloseUI();
        onComplete?.Invoke(false);
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);
        if (obj.Equals(btn_ok))
        {
            OnOkClick();
        }
        else if (obj.Equals(btn_cancel)) 
        {
            OnCancelClick();
        }
    }
}
