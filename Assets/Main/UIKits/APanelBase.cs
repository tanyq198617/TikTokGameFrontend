using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal abstract class APanelBase
{
    public bool isFilm = false;
    public bool m_IsKeepOpen = false;
    public bool m_IsLoadFromResources = false;
    public bool m_IsAlwaysOpen = false;
    public bool NeedAnchor = false;//刘海屏幕适配

    public string m_strPanelViewName = null;
    public GameObject m_gameobj = null;
    public RectTransform Trans = null;
    public Action<APanelBase> OnCloseEvent = null;
    public Action<APanelBase> OnOpenEvent = null;
    public static Vector3 HidePos = Vector3.left * 10000.0f;
    private Vector3 _srcLocalPos;

    public APanelBase()
    {
    }

    /// <summary>
    ///界面打开对挂机战斗有影响
    /// </summary>
    /// <returns></returns>
    public virtual bool IsRelateBattleUIEffect()
    {
        return true;
    }

    public string Name
    {
        private set;
        get;
    }

    public virtual void Refer() { }
    public virtual void Init() { }

    public void ShowUI(string uiName, Action<APanelBase> openCall, Action<APanelBase> closeCall)
    {
        this.Name = uiName;
        this.OnOpenEvent += openCall;
        this.OnCloseEvent += closeCall;
        ShowUI(uiName).Forget();
    }

    /// <summary>
    /// 打开UI界面，不要重载这个函数，用重载Refresh替代，本函数将取消虚方法
    /// </summary>
    /// <param name="uiName"></param>
    internal virtual async UniTask ShowUI(string uiName)
    {
        UniTask.Action(PreLoadUI).Invoke();

        await UniTask.WaitUntil(() => null != m_gameobj);

        if (m_gameobj == null)
            return;

        m_gameobj.SetActive(true);
        AddEventListener();
        Trans.SetAsLastSibling();
        Refresh();
        OnOpenEvent?.Invoke(this);
        OnOpenEvent = null;
    }

    internal virtual async UniTaskVoid PreLoadUI()
    {
        if (m_gameobj == null)
        {
            var prefab = await Resources.LoadAsync<GameObject>($"Prefabs/{m_strPanelViewName}") as GameObject;
            GameObject instance = GameObject.Instantiate(prefab);
            BindObj(instance);
            prefab = null;
        }
    }

    public void BindObj(GameObject uiobj)
    {
        if (uiobj == null)
            return;

        uiobj.transform.SetParent(UIRoot.Instance.CanvasRoot, false);
        m_gameobj = uiobj;
        m_gameobj.name = m_strPanelViewName;
        Trans = m_gameobj.GetComponent<RectTransform>();
        Trans.anchoredPosition = Vector2.zero;
        Trans.localScale = Vector3.one;

        Init();
        m_gameobj.SetActive(false);
    }

    public bool IsHasBeenOpened { get { return m_gameobj != null; } }

    public bool IsOpen { get { return IsHasBeenOpened && m_gameobj.activeInHierarchy; } }

    public virtual void CloseUI()
    {
        EndPauseHide(true);
        m_gameobj?.SetActive(false);
        OnCloseEvent?.Invoke(this);
        OnCloseEvent = null;
        RemoveEventListener();
    }

    public virtual void StartPauseHide()
    {
        if (Trans != null)
        {
            _srcLocalPos = Trans.anchoredPosition;
            Trans.anchoredPosition = HidePos;
        }
        m_IsKeepOpen = true;
    }

    public virtual void EndPauseHide(bool close = false)
    {
        if (Trans != null)
        {
            Trans.anchoredPosition = _srcLocalPos;
        }
        m_IsKeepOpen = false;
    }

    public virtual void Refresh()
    {
        ResetUI();
    }

    public virtual void Refresh<T>(T data) { ResetUI(); }

    public virtual void Update() { }

    public virtual void LateUpdate() { }

    public virtual void Destroy()
    {
        if(IsOpen) CloseUI();
        if (null != m_gameobj)
        {
            GameObject.Destroy(m_gameobj);
            m_gameobj = null;
        }
    }

    public virtual void ResetUI() { }
    public virtual void AddEventListener() { }
    public virtual void RemoveEventListener() { }
    public virtual void OnUpdateAnchor() { }
    public virtual void OnClick(GameObject obj, PointerEventData eventData) { }
    public virtual void OnPress(GameObject obj, bool isPress) { }
    public virtual void Clear() { }

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
}