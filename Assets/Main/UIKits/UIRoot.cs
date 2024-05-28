using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/// <summary>
/// 此处分级处于主项目工程
/// 改动则需要重新出包
/// </summary>
public enum UIPanelType : sbyte
{
    Battle, //战斗场景
    One,    //一级界面
    Train,  //训练界面
    Two,    //二级界面
    Three,  //三级界面
    Mask,   //上层遮罩界面
    Guide,  //新手引导界面
    Other,  //其他通用弹出
}

public class UIRoot : MonoBehaviour
{
    public static UIRoot Instance { get; private set; }

    public RectTransform CanvasRoot { get; private set; }
    public CanvasScaler UICanvasScaler { get; private set; }
    public Camera UICamera { get; private set; }

    public UniversalAdditionalCameraData URPRender { get; private set;}


    private readonly Dictionary<UIPanelType, GameObject> _roots = new Dictionary<UIPanelType, GameObject>();

    public const string RootName = "UI Root";


    public void Awake()
    {
        CanvasRoot = UIUtility.Control<RectTransform>("Canvas", transform);
        UICanvasScaler = CanvasRoot.GetComponent<CanvasScaler>();
        UICamera = GetComponent<Camera>();
        URPRender = GetComponent<UniversalAdditionalCameraData>();
        Instance = this;
        DontDestroyOnLoad(this);
    }

    internal static async UniTaskVoid InitRoot()
    {
        var obj = await Resources.LoadAsync<GameObject>($"Prefabs/UIRoot");
        GameObject prefab = obj as GameObject;
        GameObject go = GameObject.Instantiate(prefab);
        go.AddComponent<UIRoot>();
        go.name = RootName;
        UIMgr.Instance.ShowUI(UIPanelName.VersionView,openCall: (p) => Boot.Instance.OnStart());
    }

    public void OnHotfixInitialize() 
    {
        for (UIPanelType i = UIPanelType.Battle; i <= UIPanelType.Other; i++)
        {
            GameObject go = new GameObject(i.ToString());
            go.layer = LayerMask.NameToLayer("UI");
            go.transform.SetParent(CanvasRoot.transform, false);
            RectTransform rect = go.AddComponent<RectTransform>();
            SetRectTransform(rect);
            _roots.Add(i, go);
        }
    }

    public void ReleaseMainUI() => UIMgr.Instance.DestoryAll();


    private void SetRectTransform(RectTransform rect)
    {
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
    }

    public GameObject GetRoot(UIPanelType type)
    {
        GameObject go = null;
        _roots.TryGetValue(type, out go);
        return go;
    }

    public RectTransform GetRectRoot(UIPanelType type)
    {
        GameObject go = null;
        _roots.TryGetValue(type, out go);
        if (go == null) return null;
        return go.GetComponent<RectTransform>();
    }

    public void Attach(UIPanelType type, GameObject obj)
    {
        UIUtility.Attach(GetRectRoot(type), obj);
    }

    public GameObject CreateAndAttach(UIPanelType type, GameObject prefab)
    {
        GameObject obj = GameObject.Instantiate<GameObject>(prefab, CanvasRoot);
        UIUtility.Attach(GetRectRoot(type), obj);
        return obj;
    }

    public Transform Find(string path)
    {
        if (string.IsNullOrEmpty(path))
            return null;

        if (path.StartsWith(RootName))
            path = path.Replace(RootName + "/", "");
        return transform.Find(path);
    }

    public void OnSetActive(UIPanelType type, bool active)
    {
        if (_roots.TryGetValue(type, out var node))
        {
            node.SetActiveEX(active);
        }
    }

    public void SetUrpRenderType(CameraRenderType type)
    {
        URPRender.renderType = type;
    }

    public void SetDepth(int depth)
    {
        UICamera.depth = depth;
    }
}
