using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using YooAsset;

public enum ResReleaseType
{
    Non = 0,      //未打开初始化
    Once,         //使用一次后马上释放
    Auto,         //自动增加和减少
    Always,       //永驻停留
}


public class APanelBase : IEvent
{
    //UI预设值
    public bool isFilm = false;
    public bool NeedAnchor = false;//刘海屏幕适配
    public UIPanelType m_Type = UIPanelType.Two;

    //UI开启关闭时间
    public event Action OnCloseCall = null;
    public event Action OnOpenCall = null;
    public event Action<APanelBase> OnCloseEvent = null;
    public event Action<APanelBase> OnOpenEvent = null;

    //保持UI永久开启
    public bool m_IsKeepOpen = false;
    public static Vector3 HidePos = Vector3.left * 10000.0f;

    /// <summary> 声音策略 </summary>
    public UISoundControl m_Sound { get; set; }

    //下载的资源包
    private AssetOperationHandle operation;
    private bool isLoading = false;

    public GameObject m_gameobj = null;
    public RectTransform Trans = null;
    public string m_strPanelViewName = null;
    private bool isListening = false;

    public string Name { get; private set; }

    public virtual void Init() { }

    public APanelBase()
    {
        this.m_Sound = new UISoundControl(this);
    }

    public void ShowUI(Action<APanelBase> openCall, Action<APanelBase> closeCall)
    {
        this.OnOpenEvent += openCall;
        this.OnCloseEvent += closeCall;
        ShowUI().Forget();
    }

    public void ShowUI(Action openCall, Action closeCall, Action<APanelBase> closeEvent)
    {
        this.OnOpenCall += openCall;
        this.OnCloseCall += closeCall;
        this.OnCloseEvent += closeEvent;
        ShowUI().Forget();
    }

    /// <summary> 打开UI界面 </summary>
    private async UniTaskVoid ShowUI()
    {
        bool ok = await PreLoadUI();
        if (!ok) return;
        await UniTask.WaitUntil(() => null != m_gameobj);
        m_gameobj.SetActiveEX(true);
        OnBeginListener();
        PlayOpenSound();
        Trans.SetAsLastSibling();
        Refresh();
        OnOpenEvent?.Invoke(this);
        OnOpenCall?.Invoke();
        EventMgr.Dispatch(UIEvent.UI_Panel_Open, m_strPanelViewName);
    }

    public async UniTask<bool> PreLoadUI()
    {
        if (!isLoading && m_gameobj == null)
        {
            isLoading = true;
            var path = PathConst.GetUIPath(m_strPanelViewName);
            this.operation = YooAssets.LoadAssetAsync<GameObject>(path);
            await operation.ToUniTask();            
            GameObject instance = operation.InstantiateSync();
            BindObj(instance);
            if (!isLoading) return false;
            isLoading = false;
        }
        return true;
    }

    public void BindObj(GameObject uiobj)
    {
        if (uiobj == null) return;
        GameObject rootObj = UIRoot.Instance.GetRoot(m_Type);
        if (rootObj == null) return;
        m_gameobj = uiobj;
        uiobj.transform.SetParent(rootObj.transform, false);
        m_gameobj.name = m_strPanelViewName;
        Trans = m_gameobj.GetComponent<RectTransform>();
        Trans.anchoredPosition = Vector2.zero;
        Trans.localScale = Vector3.one;
        Init();
        m_gameobj.SetActiveEX(false);
    }


    public virtual void Refresh()
    {
        EventMgr.Dispatch(UIEvent.UI_Panel_Refresh, m_strPanelViewName);
        ResetUI();
    }

    public virtual void CloseUI()
    {
        if (isLoading)
            Debug.LogError($"[UI错误] 尝试关闭正在打开的UI --> {m_strPanelViewName}");
        if(!IsOpen) 
            return;
        isLoading = false;
        ObjectUtility.Safe_ActiveObj(m_gameobj, false);
        OnCloseEvent?.Invoke(this);
        OnCloseCall?.Invoke();
        SetEventNull();
        PlayCloseSound();
        OnEndListener();
        EventMgr.Dispatch(UIEvent.UI_Panel_Close, m_strPanelViewName);
    }

    public void SetEventNull() 
    {
        OnOpenEvent = null;
        OnCloseEvent = null;
        OnCloseCall = null;
        OnOpenCall = null;
    }

    public int GetCurrentIndex() => Trans != null ? Trans.GetSiblingIndex() : 0;
    public virtual void Refresh<T>(T data) { ResetUI(); }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void ResetUI() { }

    public virtual void OnClick(GameObject obj, PointerEventData eventData) { PlayButtonSound(obj); }
    public virtual void OnPress(GameObject obj, bool isPress) { }
    public virtual void Clear() { }

    //声音
    public virtual void PlayOpenSound() { m_Sound.PlayOpenSound(); }
    public virtual void PlayCloseSound() { m_Sound.PlayCloseSound(); }
    public virtual void PlayButtonSound(GameObject obj) { m_Sound.PlayButtonSound(obj); }

    private void OnBeginListener()
    {
        if (!isListening)
        {
            AddEventListener();
            isListening = true;
        }
    }

    private void OnEndListener()
    {
        if (isListening)
        {
            RemoveEventListener();
            isListening = false;
        }
    }

    //事件
    public virtual void AddEventListener() {  }
    public virtual void RemoveEventListener() {  }
    public virtual void OnUpdateAnchor() { }

    public bool IsActive
    {
        set
        {
            if (m_gameobj != null)
                m_gameobj.SetActive(value);
        }
        get
        {
            if (m_gameobj != null)
                return m_gameobj.activeInHierarchy;
            return false;
        }
    }
    public bool IsHasBeenOpened { get { return m_gameobj != null; } }
    public bool IsOpen { get { return IsHasBeenOpened && m_gameobj.activeInHierarchy; } }

    public void Destroy()
    {
        if (IsOpen) CloseUI();
        if (IsHasBeenOpened)
        {
            GameObject.Destroy(m_gameobj);
            m_gameobj = null;
        }
        operation?.Release();
        operation = null;
    }
}
