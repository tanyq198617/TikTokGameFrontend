using UnityEngine;
using TMPro;
using DG.Tweening;

[UIBind(UIPanelName.PopView)]
public class PopView : APanelBase
{
    public static PopView Instance { get { return Singleton<PopView>.Instance; } }

    public PopView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.Other;
    }

    private TextMeshProUGUI lb_tips;

    private RectTransform rect;

    private Sequence tween;
    private Vector3 from = new Vector3(1, 0, 1);
    private Vector3 to = new Vector3(1, 1, 1);

    public override void Init()
    {
        base.Init();

        lb_tips = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "lb_content");

        rect = UIUtility.Control("di", Trans);
    }

    public void IsDevelop(string name) => Show($"[{name}] 正在施工中...");

    public void Show(string str, float time = 0.5f)
    {
        if (!IsOpen)
        {
            UIMgr.Instance.ShowUI(UIPanelName.PopView, openCall: (p) =>
            {
                RefreshContent(str, time);
            });
        }
        else
            RefreshContent(str, time);
    }

    private void RefreshContent(string str, float time)
    {
        UIUtility.Safe_UGUI(ref lb_tips, str);
        ClearTween();

        rect.localScale = from;
        tween = DOTween.Sequence();
        tween.Append(DOTween.ToAxis(() => rect.localScale, x => rect.localScale = x, 1, 0.15f, AxisConstraint.Y)).AppendInterval(2);
        tween.Append(DOTween.ToAxis(() => rect.localScale, x => rect.localScale = x, 0, 0.15f, AxisConstraint.Y));
        tween.OnComplete(CloseUI);
    }

    private void ClearTween()
    {
        if (tween.IsNotNull())
        {
            tween.Kill();
            tween = null;
            rect.localScale = from;
        }
    }

    public override void CloseUI()
    {
        base.CloseUI();
        ClearTween();
    }
}
