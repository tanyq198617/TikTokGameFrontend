using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[UIBind(UIPanelName.JiGuangLoginView)]
public class JiGuangLoginView : APanelBase
{
    public static JiGuangLoginView Instance { get { return Singleton<JiGuangLoginView>.Instance; } }

    public JiGuangLoginView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }

    private GameObject btn_enter;
    private TextMeshProUGUI tx_next;

    private TMP_InputField input_acc;
    private GameObject m_tips;

    private GameObject btn_xieyi;
    private TextMeshProUGUI lb_xieyi;

    private AsToggle tg_agree;
    private RectTransform m_xieyi;
    private Sequence tween;
    public Func<bool> onAutoLogin { get; set; }

    public override void Init()
    {
        base.Init();

        input_acc = UIUtility.GetComponent<TMP_InputField>(Trans, "ipt_account");
        input_acc.onValueChanged.AddListener(OnValueChanged);

        btn_enter = UIUtility.BindClickEvent(Trans, "btn_enter", OnClick);
        tx_next = UIUtility.GetComponent<TextMeshProUGUI>(btn_enter.GetComponent<RectTransform>(), "Text");

        m_tips = UIUtility.Control("tips", m_gameobj);
        m_xieyi = UIUtility.Control("xieyi", Trans);
        tg_agree = UIUtility.BindToggleClick(Trans, "tg_agree", OnToggleClick);
        btn_xieyi = UIUtility.BindClickEvent(m_xieyi, "lb_xieyi", OnClick);
        lb_xieyi = btn_xieyi.GetComponent<TextMeshProUGUI>();
    }

    private void OnToggleClick(bool agree)
    {
        tg_agree.isOn = agree;
        LocalDataMgr.SetInt(LocalKey.AgreeKey, agree ? 1 : 0);
    }

    private void OnValueChanged(string str)
    {
        UIUtility.Safe_Color(ref tx_next, string.IsNullOrEmpty(str) ? ColorConst.UnActiveTextColor : ColorConst.ActiveTextColor);
    }

    public override void Refresh()
    {
        base.Refresh();

        m_tips.SetActiveEX(false);
        tg_agree.isOn = LocalDataMgr.GetInt(LocalKey.AgreeKey) == 1;

        if (onAutoLogin != null)
        {
            string openId = LocalDataMgr.GetStr(LocalKey.OpenID);
            if (!string.IsNullOrEmpty(openId))
            {
                UIUtility.Safe_UGUI(ref input_acc, openId);
            }
            else
            {
                onAutoLogin = null;
                UIUtility.Safe_UGUI(ref input_acc, "", true);
            }
        }
    }

    /// <summary>
    /// 检测是否输入账号和密码
    /// </summary>
    /// <returns></returns>
    private bool CheckAccAndPwd()
    {
        if (string.IsNullOrEmpty(input_acc.text))
        {
            //ComHelpView.Instance.Show(LocalizationMgr.Instance.GetString("LoginView_AccOrPassNull"));
            return false;
        }
        return true;
    }

    private void OnLogin()
    {
        //if (!NetMgr.Instance.IsConnected)
        //    return;

        string phoneNum = input_acc.text;
        string phoneStr = phoneNum.StartsWith("+86") ? phoneNum.Replace("+86", "") : phoneNum;

        if (onAutoLogin != null)
        {
            string openId = LocalDataMgr.GetStr(LocalKey.OpenID);
            openId = openId.StartsWith("+86") ? openId.Replace("+86", "") : openId;
            if (phoneStr.Equals(openId) && onAutoLogin.Invoke())
            {
                return;
            }
        }

        if (!CheckAccAndPwd())
            return;

        if (!tg_agree.isOn)
        {
            m_tips.SetActiveEX(true);
            tween = DOTween.Sequence();
            tween.Append(tg_agree.transform.DOScale(Vector3.one * 2f, 0.1f));
            tween.Append(tg_agree.transform.DOScale(Vector3.one, 0.1f));
            tween.AppendInterval(2);
            tween.OnComplete(() => { m_tips.SetActiveEX(false); });
            return;
        }

        //登录按钮点击
        //EventMgr.Dispatch(LoginEvent.JiGuang_Login, str);
        EventMgr.Dispatch(SDKEvent.JiGuang_Phone_GetKey, phoneStr);
        WaitCircleView.Instance.OnLock();
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_enter))
        {
            OnLogin();
        }
        else if (obj.Equals(btn_xieyi))
        {
            lb_xieyi?.TMP_TextLinkClick(eventData, (id, text) =>
            {
                OnOpenYinsi();
            });
        }
    }

    private void OnOpenYinsi()
    {
        PrivacyView.Instance.OnOpenUI((b) =>
        {
            OnToggleClick(b);
        });
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<int, string>(SDKEvent.JiGuang_Phone_GetCode_CallBack, OnGetCode);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<int, string>(SDKEvent.JiGuang_Phone_GetCode_CallBack, OnGetCode);
    }

    private void OnGetCode(int code, string verificationId)
    {
        Debuger.LogError($"收到服务器验证码：{verificationId}");
        if (code == 3000)
        {
            JiGuangCodeLoginView.Instance.Show(input_acc.text, verificationId);
        }
        WaitCircleView.Instance.UnLock();
    }
}
