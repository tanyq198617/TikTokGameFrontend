using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightTopButtonsPage : AItemPageBase
{

    private GameObject btn_shouchong;
    private GameObject btn_qiri;
    private GameObject btn_leideng;
    private GameObject btn_huodong;
    private GameObject btn_tehui;
    private GameObject btn_xinjian;
    private GameObject btn_qiandao;
    private GameObject btn_zaixian;
    private GameObject btn_chengjiu;
    private GameObject btn_fuli;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        btn_shouchong = UIUtility.BindClickEvent(RectTrans, "btn_shouchong", OnClick);
        btn_qiri = UIUtility.BindClickEvent(RectTrans, "btn_qiri", OnClick);
        btn_leideng = UIUtility.BindClickEvent(RectTrans, "btn_leideng", OnClick);
        btn_huodong = UIUtility.BindClickEvent(RectTrans, "btn_huodong", OnClick);
        btn_tehui = UIUtility.BindClickEvent(RectTrans, "btn_tehui", OnClick);
        btn_xinjian = UIUtility.BindClickEvent(RectTrans, "btn_xinjian", OnClick);
        btn_qiandao = UIUtility.BindClickEvent(RectTrans, "btn_qiandao", OnClick);
        btn_zaixian = UIUtility.BindClickEvent(RectTrans, "btn_zaixian", OnClick);
        btn_chengjiu = UIUtility.BindClickEvent(RectTrans, "btn_chengjiu", OnClick);
        btn_fuli = UIUtility.BindClickEvent(RectTrans, "btn_fuli", OnClick);

    }

    
    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_shouchong)) 
        {
            PopView.Instance.IsDevelop("首充");
        }
        else if (obj.Equals(btn_qiri))
        {
            PopView.Instance.IsDevelop("七日");
        }
        else if (obj.Equals(btn_leideng))
        {
            PopView.Instance.IsDevelop("累登");
        }
        else if (obj.Equals(btn_huodong))
        {
            PopView.Instance.IsDevelop("活动");
        }
        else if (obj.Equals(btn_tehui))
        {
            PopView.Instance.IsDevelop("特惠");
        }
        else if (obj.Equals(btn_xinjian))
        {
            PopView.Instance.IsDevelop("信件");
        }
        else if (obj.Equals(btn_qiandao))
        {
            PopView.Instance.IsDevelop("签到");
        }
        else if (obj.Equals(btn_zaixian))
        {
            PopView.Instance.IsDevelop("在线");
        }
        else if (obj.Equals(btn_chengjiu))
        {
            PopView.Instance.IsDevelop("成就");
        }
        else if (obj.Equals(btn_fuli))
        {
            PopView.Instance.IsDevelop("福利");
        }

    }
}
