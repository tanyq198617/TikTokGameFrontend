using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

internal class VersionView : APanelBase, IProgress<float>
{
    public static VersionView Instance { get { return Singleton<VersionView>.Instance; } }

    public VersionView() : base()
    {
        isFilm = true;
        m_IsLoadFromResources = true;
    }

    private TextMeshProUGUI tx_progress;
    private TextMeshProUGUI tx_version;
    private Slider progress;
    private GameObject btn_clear;
    private GameObject btn_start;

    public override void Init()
    {
        base.Init();

        progress = UIUtility.GetComponent<Slider>(Trans, "progress");
        tx_progress = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_progress");
        tx_version = UIUtility.GetComponent<TextMeshProUGUI>(Trans, "tx_version");
        btn_clear = UIUtility.BindClickEvent(Trans, "btn_clear", OnClick);
        btn_start = UIUtility.BindClickEvent(Trans, "btn_start", OnClick);
    }

    private void SetVersion(string version, string resVersion) => UIUtility.Safe_UGUI(ref tx_version, $"v{version} {resVersion}");
    private void OnStartUpdate() { progress.SetActiveEX(true); btn_clear.SetActiveEX(false); }
    private void OnEndUpdate() { progress.SetActiveEX(false); btn_clear.SetActiveEX(true); }

    private void OnDownloadProgressCallback(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        UIUtility.Safe_UGUI(ref tx_progress, $"当前下载个数：{currentDownloadCount}/{totalDownloadCount}, 大小:{currentDownloadBytes.ToSizeStr()}/{totalDownloadBytes.ToSizeStr()}");
        UIUtility.Safe_Float(ref progress, currentDownloadBytes * 1.0f / totalDownloadBytes);
    }

    public void Report(float value) => UIUtility.Safe_Float(ref progress, value);


    private void TryClear()
    {
        Boot.TryClear();
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);
        if (obj.Equals(btn_clear)) 
        {
            TryClear();
        }
        else if (obj.Equals(btn_start))
        {
            btn_start.SetActiveEX(false);
            Boot.Instance.OnStart();
        }
    }

    public override void AddEventListener()
    {
        base.AddEventListener();
        EventMgr.AddEventListener<string, string>(MainEvent.App_Version, SetVersion);
        EventMgr.AddEventListener(MainEvent.Assets_Progress_Start, OnStartUpdate);
        EventMgr.AddEventListener(MainEvent.Assets_Progress_End, OnEndUpdate);
        EventMgr.AddEventListener<int,int,long,long>(nameof(OnDownloadProgressCallback), OnDownloadProgressCallback);
    }

    public override void RemoveEventListener()
    {
        base.RemoveEventListener();
        EventMgr.RemoveEventListener<string, string>(MainEvent.App_Version, SetVersion);
        EventMgr.RemoveEventListener(MainEvent.Assets_Progress_Start, OnStartUpdate);
        EventMgr.RemoveEventListener(MainEvent.Assets_Progress_End, OnEndUpdate);
        EventMgr.RemoveEventListener<int, int, long, long>(nameof(OnDownloadProgressCallback), OnDownloadProgressCallback);
    }
}
