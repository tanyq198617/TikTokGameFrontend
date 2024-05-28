using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[UIBind(UIPanelName.JiGuangCodeLoginView)]
public class JiGuangCodeLoginView : APanelBase
{
    public static JiGuangCodeLoginView Instance { get { return Singleton<JiGuangCodeLoginView>.Instance; } }

    public JiGuangCodeLoginView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }

    private TextMeshProUGUI lb_phone;
    private TextMeshProUGUI lb_cd;

    private Image img_enter;
    private TextMeshProUGUI lb_enter;

    private TMP_InputField input_code;
    private GameObject input_Caret;

    private GameObject btn_back;
    private GameObject btn_enter;
    private GameObject btn_reget;

    private string phoneNum;
    private string m_verificationId;
    private string code;

    private int codeLength = 6;
    private int codeTime = 120;

    private TickedBase _tick;
    private float timer;

    public override void Init()
    {
        base.Init();

        lb_phone = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "lb_phone");
        lb_cd = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "lb_cd");

        input_code = UIUtility.GetComponent<TMP_InputField>(Trans, "input_code");
        input_code.onValueChanged.AddListener(OnValueChanged);
        input_Caret = UIUtility.Control("Caret", input_code.gameObject);

        btn_back = UIUtility.BindClickEvent(Trans, "btn_back", OnClick);
        btn_enter = UIUtility.BindClickEvent(Trans, "btn_enter", OnClick);
        btn_reget = UIUtility.BindClickEvent(Trans, "lb_cd", OnClick);

        lb_enter = UIUtility.GetComponent<TextMeshProUGUI>(btn_enter, "Text");
        img_enter = UIUtility.GetComponent<Image>(btn_enter, "Image");
    }

    private void OnValueChanged(string value)
    {
        CheckState();
    }

    private void CheckState()
    {
        bool hasValue = !string.IsNullOrEmpty(input_code.text) && input_code.text.Length >= codeLength;
        if (input_Caret == null)
            input_Caret = UIUtility.Control("Caret", input_code.gameObject);
        input_Caret.SetActiveEX(input_code.text.Length < codeLength);    //光标
        UIUtility.Safe_Color(ref lb_enter, hasValue ? ColorConst.UnActiveTextColor : ColorConst.ActiveTextColor);
        UIUtility.Safe_Color(ref img_enter, hasValue ? ColorConst.ActiveTextColor : ColorConst.UnActiveTextColor);
    }

    public override void Refresh()
    {
        base.Refresh();
        input_code.ResetUI();
    }

    private void CreateTick()
    {
        if (_tick == null)
            _tick = TickedBase.Create(1, OnTick, false, true);
    }

    public void Show(string phoneNum, string verificationId)
    {
        if (!IsOpen)
        {
            UIMgr.Instance.ShowUI(m_strPanelViewName, openCall: (p) =>
            {
                RefreshContent(phoneNum, verificationId);
            });
        }
        else
        {
            RefreshContent(phoneNum, verificationId);
        }
    }

    private void RefreshContent(string phoneNum, string verificationId)
    {
        this.phoneNum = phoneNum;
        this.m_verificationId = verificationId;
        this.timer = TimeMgr.RealTime;
        lb_cd.raycastTarget = false;
        input_code.ResetUI();
        this.CreateTick();

        string phoneStr = phoneNum.StartsWith("+86") ? phoneNum : $"+86 {phoneNum}";
        UIUtility.Safe_UGUI(ref lb_phone, phoneStr);
    }

    private void OnTick()
    {
        int time = Mathf.RoundToInt(TimeMgr.RealTime - timer);

        if (time <= codeTime)
        {
            //Debuger.LogError(time);
            UIUtility.Safe_UGUI(ref lb_cd, $"重新获取 ({codeTime - time}秒后)");
        }
        else
        {
            UIUtility.Safe_UGUI(ref lb_cd, $"重新获取");
            lb_cd.raycastTarget = true;
            ClearTick();
        }
    }

    /// <summary>
    /// 重新获取
    /// </summary>
    private void OnReGetCode()
    {
        string phoneStr = phoneNum.StartsWith("+86") ? phoneNum.Replace("+86", "") : phoneNum;
        EventMgr.Dispatch(SDKEvent.JiGuang_Phone_GetKey, phoneStr);
    }

    /// <summary>
    /// 登录
    /// </summary>
    private void OnLogin()
    {
        if (!phoneNum.IsRegexMatch(RegexConst.phone))
        {
            Debuger.LogError($"手机号不符合规则");
            return;
        }
        string code = input_code.text;
        if (!code.IsRegexMatch(RegexConst.number))
        {
            Debuger.LogError($"验证码不符合规则");
            return;
        }
        EventMgr.Dispatch(SDKEvent.JiGuang_Phone_Login, phoneNum, code);
    }

    private void ClearTick()
    {
        if (_tick != null)
        {
            TickedMgr.Instance.Remove(_tick);
            _tick = null;
        }
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ClearTick();
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_back))
        {
            CloseUI();
        }
        else if (obj.Equals(btn_enter))
        {
            OnLogin();
        }
        else if (obj.Equals(btn_reget))
        {
            OnReGetCode();
        }
    }
}