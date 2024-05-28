using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[UIBind(UIPanelName.LoginView)]
public class LoginView : APanelBase
{
    public static LoginView Instance { get { return Singleton<LoginView>.Instance; } }

    public LoginView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }


    private TMP_InputField input_account;
    private TextMeshProUGUI tx_tips;
    private GameObject btn_enter;
    private RectTransform m_tips;
    private AsToggle tg_agree;
    private Sequence tween;


    public override void Init()
    {
        base.Init();

        input_account = UIUtility.GetComponent<TMP_InputField>(Trans, "ipt_account");
        tx_tips = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_tips");

        m_tips = UIUtility.Control("tips", Trans);
        tg_agree = UIUtility.BindToggleClick(Trans, "tg_agree", OnToggleClick);
        btn_enter = UIUtility.BindClickEvent(Trans, "btn_enter", OnClick);
    }


    private void OnEnterGame()
    {
        NetMgr.GetHandler<LoginHandler>().ReqLogin();
        // UIMgr.Instance.ShowUI(UIPanelName.TestView);
        if (string.IsNullOrEmpty(input_account.text)) 
        {
            ShowMessage("请输入手机号码");
            return;
        }
        // NetMgr.GetHandler<LoginHanlder>()?.OnReqLogin(input_account.text, input_account.text, 0);
    }

    private void OnToggleClick(bool value)
    {
    }

    private void ShowMessage(string msg)
    {
        tween?.Kill(false);
        tween = null;
        UIUtility.Safe_UGUI(ref tx_tips, msg);
        m_tips.SetActiveEX(true);
        tween = DOTween.Sequence();
        tween.Append(m_tips.DOShakeAnchorPos(0.5f, 2f, snapping: true));
        tween.AppendInterval(5);
        tween.onComplete = () => 
        {
            m_tips.SetActiveEX(false);
        };
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
