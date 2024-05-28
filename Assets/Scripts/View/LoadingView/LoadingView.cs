using System;
using TMPro;
using UnityEngine.UI;

[UIBind(UIPanelName.LoadingView)]
public class LoadingView : APanelBase
{
    public static LoadingView Instance { get { return Singleton<LoadingView>.Instance; } }

    public LoadingView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.Other;
        m_IsKeepOpen = true;
    }

    private bool isOpening = false;
    private float curProgerss;
    private Slider progressBar;
    private TextMeshProUGUI perText;

    public override void Init()
    {
        base.Init();
        progressBar = UIUtility.GetComponent<Slider>(Trans, "ProgressBar");
        perText = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "Percent");
    }

    public override void Refresh()
    {
        base.Refresh();
        isOpening = false;
        curProgerss = 0;
        RefreshProgressBar();
    }

    /// <summary>
    /// 设置当前加载进度
    /// </summary>
    /// <param name="num"></param>
    public void SetProgress(float progress)
    {
        curProgerss = progress;
        if (!IsOpen)
        {
            return;
        }
        RefreshProgressBar();
    }

    /// <summary>
    /// 打开加载面板
    /// </summary>
    public void OpenUI()
    {
        if (isOpening)
            return;
        if (IsOpen)
        {
            Refresh();
            return;
        }
        UIMgr.Instance.ShowUICall(UIPanelName.LoadingView);
        isOpening = true;
    }

    /// <summary>
    /// 刷新进度条
    /// </summary>
    private void RefreshProgressBar()
    {
        progressBar.value = curProgerss;
        perText.text = $"{Math.Floor(curProgerss * 100)}%";
    }

    /// <summary>
    /// 进入游戏场景了
    /// </summary>
    private void OnEnterStage()
    {
        CloseUI();
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener(StageEvent.Stage_Enter_Finished, OnEnterStage);
        EventMgr.AddEventListener<string>(UIEvent.UI_Panel_Open, OnPanelOpen);
        EventMgr.AddEventListener<int, int, string>(GameEvent.NodeStartChange, OnNodeChanged);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener(StageEvent.Stage_Enter_Finished, OnEnterStage);
        EventMgr.RemoveEventListener<string>(UIEvent.UI_Panel_Open, OnPanelOpen);
        EventMgr.RemoveEventListener<int, int, string>(GameEvent.NodeStartChange, OnNodeChanged);
    }

    private void OnNodeChanged(int index, int count, string nodeName)
    {
        SetProgress(index * 1.0f / count);
    }



    private void OnPanelOpen(string panelName)
    {
        //if (panelName.Equals(UIPanelName.LoginView) || panelName.Equals(UIPanelName.SdkLoginView))
        //{
        //    CloseUI();
        //}
    }
}
