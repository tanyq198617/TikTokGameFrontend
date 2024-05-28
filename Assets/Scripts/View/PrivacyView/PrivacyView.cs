using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[UIBind(UIPanelName.PrivacyView)]
public class PrivacyView : APanelBase
{
    public static PrivacyView Instance { get { return Singleton<PrivacyView>.Instance; } }

    public PrivacyView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.Two;
    }

    private GameObject btn_ok;
    private GameObject btn_cancel;

    private ScrollRect m_Scroll;
    private RectTransform mContentTrans;

    private Action<bool> OnSelectHandler;


    public override void Init()
    {
        base.Init();

        m_Scroll = UIUtility.GetComponent<ScrollRect>(Trans, "Scroll View");
        mContentTrans = UIUtility.GetComponent<RectTransform>(Trans, "Content");

        btn_ok = UIUtility.BindClickEvent(Trans, "button_ok", OnClick);
        btn_cancel = UIUtility.BindClickEvent(Trans, "button_no", OnClick);
    }

    public override void Refresh()
    {
        base.Refresh();
        ResetPosition();
    }

    public void OnOpenUI(Action<bool> selectHandler)
    {
        this.OnSelectHandler = selectHandler;

        if (!IsOpen)
            UIMgr.Instance.ShowUI(UIPanelName.PrivacyView);
        else
            Refresh();
    }

    private void ResetPosition()
    {
        m_Scroll.StopMovement();
        mContentTrans.pivot = new Vector2(0.5f, 1f);
        mContentTrans.anchoredPosition = Vector2.zero;
    }

    private void OnCancelClick()
    {
        OnSelectHandler?.Invoke(false);
        CloseUI();
    }

    private void OnOkClick()
    {
        OnSelectHandler?.Invoke(true);
        CloseUI();
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
