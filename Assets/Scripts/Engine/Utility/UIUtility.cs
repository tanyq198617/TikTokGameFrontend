using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI工具集
/// </summary>
public class UIUtility
{

    #region 检测组件是否存在
    private static bool CheckComponent(GameObject obj, string parent, string name)
    {
        if (null == obj)
        {
            Debuger.LogWarning("!!预件" + parent + "下未找到" + name);
            return false;
        }
        return true;
    }

    private static bool CheckComponent(RectTransform trans, string parent, string name)
    {
        if (null == trans)
        {
            Debuger.LogWarning("!!预件" + parent + "下未找到" + name);
            return false;
        }
        return true;
    }

    private static bool CheckComponent(Transform trans, string parent, string name)
    {
        if (null == trans)
        {
            Debuger.LogWarning("!!预件" + parent + "下未找到" + name);
            return false;
        }
        return true;
    }

    private static bool IsExist(UnityEngine.GameObject comp)
    {
        if (comp == null)
        {
            Debuger.LogWarning("!!预件不存在!!!");
        }
        return comp != null;
    }
    private static bool IsExist(UnityEngine.Component comp)
    {
        if (comp == null)
        {
            Debuger.LogWarning("!!预件不存在!!!");
        }
        return comp != null;
    }
    #endregion

    #region 获取控件
    public static T GetComponent<T>(GameObject obj, string name) where T : Component
    {
        if (obj == null) return null;
        return GetComponent<T>(obj.rectTransform(), name);
    }

    public static T GetComponent<T>(RectTransform trans, string name) where T : Component
    {
        if (trans == null)
            return null;

        if (string.IsNullOrEmpty(name))
            return trans.GetComponent<T>();

        RectTransform findTrans = Control(name, trans);

        if (!CheckComponent(findTrans, trans.name, name))
        {
            return null;
        }

        return findTrans.GetComponent<T>();
    }

    public static T GetComponent<T>(Transform trans, string name) where T : Component
    {
        if (trans == null)
            return null;

        if (string.IsNullOrEmpty(name))
            return trans.GetComponent<T>();

        Transform findTrans = Control(name, trans);

        if (!CheckComponent(findTrans, trans.name, name))
        {
            return null;
        }

        return findTrans.GetComponent<T>();
    }

    public static RectTransform Control(string name, RectTransform gameObj)
    {
        if (null == gameObj || gameObj.childCount == 0)
            return null;

        for (int i = 0; i < gameObj.childCount; ++i)
        {
            RectTransform ctrans = gameObj.GetChild(i).GetComponent<RectTransform>();
            if (ctrans.name.Equals(name))
                return ctrans;
        }

        for (int i = 0; i < gameObj.childCount; ++i)
        {
            RectTransform ttTrans = Control(name, gameObj.GetChild(i).GetComponent<RectTransform>());
            if (ttTrans != null)
                return ttTrans;
        }
        return null;
    }

    public static Transform Control(string name, Transform gameObj)
    {
        if (null == gameObj || gameObj.childCount == 0)
            return null;

        for (int i = 0; i < gameObj.childCount; ++i)
        {
            Transform ctrans = gameObj.GetChild(i);
            if (ctrans.name.Equals(name))
                return ctrans;
        }

        for (int i = 0; i < gameObj.childCount; ++i)
        {
            Transform ttTrans = Control(name, gameObj.GetChild(i));
            if (ttTrans != null)
                return ttTrans;
        }
        return null;
    }

    public static GameObject Control(string name, GameObject gameObj)
    {
        if (null == gameObj) return null;
        Transform trans = Control(name, gameObj.transform);
        if (trans == null)
        {
            Debuger.LogError("查找不到游戏物体，{0}下的{1}", gameObj.name, name);
            return null;
        }
        return trans.gameObject;
    }
    #endregion

    #region 控件赋值
    public static void Safe_UGUI<T>(ref T ui, object text, bool reset = false)
    {
        if (ui == null) return;
        if (text == null && !reset)
        {
            Debuger.LogError("设定{0}数据为空！", ui.ToString());
            return;
        }
        else if (ui is Image image)
        {
            if (text == null && reset)
                image.sprite = null;
            else
                image.sprite = text as Sprite;
        }
        else if (ui is Text textComp)
        {
            if (text == null && reset)
                textComp.text = null;
            else
                textComp.text = text.ToString();
        }
        else if (ui is TMPro.TextMeshProUGUI textMeshComp)
        {
            if (text == null && reset)
                textMeshComp.text = null;
            else
                textMeshComp.text = text.ToString();
        }
        else if (ui is TMPro.TextMeshPro textMesh)
        {
            if (text == null && reset)
                textMesh.text = null;
            else
                textMesh.text = text.ToString();
        }
        else if (ui is TMPro.TMP_InputField textFieldComp)
        {
            if (text == null && reset)
                textFieldComp.text = "";
            else
                textFieldComp.text = text.ToString();
        }
        else if (ui is RawImage rawImage)
        {
            if (rawImage == null && reset)
                rawImage.texture = null;
            else
                rawImage.texture = text as Texture;
        }
    }

    public static void Safe_Float<T>(ref T ui, float value)
    {
        if (ui == null) return;

        if (ui is Slider slider)
            slider.value = value;
        else if (ui is Scrollbar scrollbar)
            scrollbar.value = value;
        else if (ui is Image image)
            image.fillAmount = value;
    }

    public static void Safe_Color<T>(ref T ui, string colorStr)
    {
        if (ui == null) return;

        if (ColorUtility.TryParseHtmlString(colorStr, out Color backColor))
            Safe_Color<T>(ref ui, backColor);
    }

    public static void Safe_Color<T>(ref T ui, Color color)
    {
        if (ui == null) return;

        if (ui is HudImageMesh mesh)
        {
            mesh.color = color;
        }
        else if (ui is Image image)
        {
            image.color = color;
        }
        else if (ui is TextMeshProUGUI tmp)
        {
            tmp.color = color;
        }
    }

    public static void Safe_Color32<T>(ref T ui, Color32 color)
    {
        if (ui == null) return;

        if (ui is HudImageMesh mesh)
        {
            mesh.color = color;
        }
        else if (ui is Image image)
        {
            image.color = color;
        }
        else if (ui is TextMeshProUGUI tmp)
        {
            tmp.color = color;
        }
    }

    public static void SetNativeSize(ref Image image, float scale = 1f)
    {
        if (image == null) return;
        image.SetNativeSize();
        if (scale != 1f)
            image.rectTransform.sizeDelta = new Vector2(image.rectTransform.sizeDelta.x * scale, image.rectTransform.sizeDelta.y * scale);
    }
    #endregion

    #region 绑定事件

    #region 点击事件
    public static GameObject BindClickEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call, string se_sound = SoundConst.SE_Click)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindClickEvent(findTrans.gameObject, call, se_sound);
    }

    public static GameObject BindClickEvent(GameObject obj, UIEventHandle<PointerEventData> call, string se_sound = SoundConst.SE_Click)
    {
        if (null == obj) return null;
        //call += (o, ev) => { SoundMgr.Instance.PlaySound(se_sound); };
        //EventTriggerListener.Get(obj).onClick.AddListener((o, ev) => SoundMgr.Instance.PlaySound(se_sound));
        EventTriggerListener.Get(obj).onClick.AddListener(delegate (GameObject o, PointerEventData ev)
        {
            SoundMgr.Instance.CrossFadeSound(se_sound);
            call?.Invoke(o, ev);
        });
        return obj;
    }
    #endregion

    #region 双击事件
    public static GameObject BindDoubleClickEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call, string se_sound = SoundConst.SE_Click)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindDoubleClickEvent(findTrans.gameObject, call, se_sound);
    }

    public static GameObject BindDoubleClickEvent(GameObject obj, UIEventHandle<PointerEventData> call, string se_sound = SoundConst.SE_Click)
    {
        if (null == obj) return null;
        //call += (o, ev) => { SoundMgr.Instance.PlaySound(se_sound); };
        //EventTriggerListener.Get(obj).onClick.AddListener((o, ev) => SoundMgr.Instance.PlaySound(se_sound));
        EventTriggerListener.Get(obj).onDoubleClick.AddListener(delegate (GameObject o, PointerEventData ev)
        {
            SoundMgr.Instance.CrossFadeSound(se_sound);
            call?.Invoke(o, ev);
        });
        return obj;
    }
    #endregion

    #region 监听值改变


    public static void BindValueChanged<T>(ref T ui, Action<object> callback)
    {
        if (ui == null) return;
        if (callback == null)
        {
            Debuger.LogError("设定{0}数据为空！", ui.ToString());
            return;
        }

        if (ui is Slider)
        {
            (ui as Slider).onValueChanged.AddListener((str) => { callback(str); });
        }
        else if (ui is Scrollbar)
        {
            (ui as Scrollbar).onValueChanged.AddListener((str) => { callback(str); });
        }
        else if (ui as TMP_Dropdown)
        {
            (ui as TMP_Dropdown).onValueChanged.AddListener((str) => { callback(str); });
        }
        else if (ui as TMP_InputField)
        {
            (ui as TMP_InputField).onValueChanged.AddListener((str) => { callback(str); });
        }
    }
    #endregion

    #region 按下事件
    public static GameObject BindPressDownEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindPressDownEvent(findTrans.gameObject, call);
    }
    public static GameObject BindPressDownEvent(GameObject obj, UIEventHandle<PointerEventData> call)
    {
        if (null == obj) return null;
        EventTriggerListener.Get(obj).onPress.AddListener(call);
        return obj;
    }
    #endregion

    #region 抬起事件
    public static GameObject BindPressUpEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindPressUpEvent(findTrans.gameObject, call);
    }
    public static GameObject BindPressUpEvent(GameObject obj, UIEventHandle<PointerEventData> call)
    {
        if (null == obj) return null;
        EventTriggerListener.Get(obj).onUp.AddListener(call);
        return obj;
    }
    #endregion

    #region 拖拽事件
    public static GameObject BindDragBeginEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindDragBeginEvent(findTrans.gameObject, call);
    }
    public static GameObject BindDragBeginEvent(GameObject obj, UIEventHandle<PointerEventData> call)
    {
        if (null == obj) return null;
        EventTriggerListener.Get(obj).onBeginDrag.AddListener(call);
        return obj;
    }

    public static GameObject BindDragEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindDragEvent(findTrans.gameObject, call);
    }
    public static GameObject BindDragEvent(GameObject obj, UIEventHandle<PointerEventData> call)
    {
        if (null == obj) return null;
        EventTriggerListener.Get(obj).onDrag.AddListener(call);
        return obj;
    }

    public static GameObject BindDragEndEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindDragEndEvent(findTrans.gameObject, call);
    }
    public static GameObject BindDragEndEvent(GameObject obj, UIEventHandle<PointerEventData> call)
    {
        if (null == obj) return null;
        EventTriggerListener.Get(obj).onEndDrag.AddListener(call);
        return obj;
    }

    public static GameObject BindScrollEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindScrollEvent(findTrans.gameObject, call);
    }
    public static GameObject BindScrollEvent(GameObject obj, UIEventHandle<PointerEventData> call)
    {
        if (null == obj) return null;
        EventTriggerListener.Get(obj).onScroll.AddListener(call);
        return obj;
    }
    #endregion


    public static GameObject BindUIEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> call)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();

        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
            trigger.triggers = new List<EventTrigger.Entry>();
        }

        Selectable select = obj.GetComponent<Selectable>();
        if (select == null)
        {
            select = obj.AddComponent<Selectable>();
            select.transition = Selectable.Transition.None;
        }

        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type, };
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(call);
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);
        return obj;
    }
    #endregion

    #region 获取Toggle组件并监听Toggle值改变
    public static AsToggle BindToggleClick(RectTransform trans, string name, UnityAction<bool> call, string se_sound = SoundConst.None)
    {
        AsToggle toggle = GetComponent<AsToggle>(trans, name);
        if (toggle != null)
        {
            toggle.onClick.AddListener((o, ev) => { SoundMgr.Instance.CrossFadeSound(se_sound); });
            toggle.onValueChanged.AddListener(call);
        }
        return toggle;
    }

    public static AsToggle BindToggleClick<T>(RectTransform trans, string name, UnityAction<bool, T> call, T param, string se_sound = SoundConst.None)
    {
        AsToggle toggle = GetComponent<AsToggle>(trans, name);
        if (toggle != null)
        {
            toggle.onClick.AddListener((o, ev) => { SoundMgr.Instance.CrossFadeSound(se_sound); });
            toggle.onValueChanged.AddListener((bool isOn) =>
            {
                call(isOn, param);
            });
        }
        return toggle;
    }
    #endregion

    #region 绑定Button点击事件
    public static Button BindButtonClick(RectTransform trans, string name, UnityAction call, string se_sound = SoundConst.SE_Click)
    {
        Button button = GetComponent<Button>(trans, name);
        if (button == null)
            return null;
        call += () =>
        {
            SoundMgr.Instance.CrossFadeSound(se_sound);
        };
        button.onClick.AddListener(call);
        return button;
    }

    public static Button BindButtonClick(GameObject obj, UnityAction call, string se_sound = SoundConst.SE_Click)
    {
        Button button = obj.GetComponent<Button>();
        if (button == null)
            return null;
        call += () =>
        {
            SoundMgr.Instance.CrossFadeSound(se_sound);
        };
        button.onClick.AddListener(call);
        return button;
    }

    public static Button BindButtonClick<T>(RectTransform trans, string name, UnityAction<T> call, T param, string se_sound = SoundConst.SE_Click)
    {
        Button button = GetComponent<Button>(trans, name);
        if (button == null)
            return null;
        button.onClick.AddListener(delegate
        {
            SoundMgr.Instance.CrossFadeSound(se_sound);
            call(param);
        });
        return button;
    }

    public static Button BindButtonClick<T>(GameObject obj, UnityAction<T> call, T param, string se_sound = SoundConst.SE_Click)
    {
        Button button = obj.GetComponent<Button>();
        if (button == null)
            return null;
        button.onClick.AddListener(delegate
        {
            SoundMgr.Instance.CrossFadeSound(se_sound);
            call(param);
        });
        return button;
    }
    #endregion

    #region 创建/克隆UI
    public static void Attach(RectTransform trans, GameObject child, bool isIncludeScale = true)
    {
        if (child == null) return;
        child.transform.SetParent(trans);
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        if (isIncludeScale) child.transform.localScale = Vector3.one;
    }

    public static void Attach(Transform trans, GameObject child, bool isIncludeScale = true)
    {
        if (child == null) return;
        child.transform.SetParent(trans);
        child.transform.localPosition = Vector3.zero;
        child.transform.localRotation = Quaternion.identity;
        if (isIncludeScale) child.transform.localScale = Vector3.one;
    }

    public static GameObject AddChild(GameObject template, Transform parent)
    {
        if (!IsExist(template) || !IsExist(parent))
        {
            return null;
        }

        GameObject go = GameObject.Instantiate(template) as GameObject;
        go.transform.SetParent(parent);
        RectTransform rect = go.GetOrAddComponent<RectTransform>();
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = Vector3.one;
        return go;
    }

    public static GameObject CreateChild(GameObject template, Transform parent, Vector3 position, bool worldPositionStays)
    {
        if (!IsExist(template) || !IsExist(parent))
        {
            return null;
        }

        GameObject go = GameObject.Instantiate(template) as GameObject;
        go.transform.position = position;
        go.transform.localScale = Vector3.one;
        go.transform.SetParent(parent, worldPositionStays);
        return go;
    }

    public static T CreateItem<T>(GameObject obj, RectTransform parent, bool isIncludeScale = true) where T : AItemBase
    {
        if (obj == null) return null;

        GameObject curObj = GameObject.Instantiate<GameObject>(obj);
        //curObj.SetActive(true);
        Attach(parent, curObj, isIncludeScale);
        T item = ActivatorFactory.CreateInstance<T>();
        item.setObj(curObj);
        return item;
    }

    public static T CreateItem<T>(GameObject obj, Transform parent) where T : AItemBase
    {
        if (obj == null) return null;

        GameObject curObj = GameObject.Instantiate<GameObject>(obj);
        //curObj.SetActive(true);
        Attach(parent, curObj, true);
        T item = ActivatorFactory.CreateInstance<T>();
        item.setObj(curObj);
        return item;
    }

    public static T CreateItemNoClone<T>(RectTransform trans, string name) where T : AItemBase
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return CreateItemNoClone<T>(findTrans.gameObject);
    }

    public static T CreateItemNoClone<T>(Transform trans, string name) where T : AItemBase
    {
        if (null == trans)
            return null;
        Transform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return CreateItemNoClone<T>(findTrans.gameObject);
    }

    public static T CreateItemNoClone<T>(GameObject obj) where T : AItemBase
    {
        if (obj == null) return null;
        //obj.SetActive(true);
        T item = ActivatorFactory.CreateInstance<T>();
        item.setObj(obj);
        return item;
    }

    public static T CreatePageNoClone<T>(RectTransform trans, string name) where T : AItemPageBase
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        GameObject obj = findTrans.gameObject;
        //obj.SetActive(true);
        T item = ActivatorFactory.CreateInstance<T>();
        item.setObj(obj);
        return item;
    }

    public static T CreatePageNoClone<T>(Transform trans, string name) where T : AItemPageBase
    {
        if (null == trans)
            return null;
        Transform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        GameObject obj = findTrans.gameObject;
        //obj.SetActive(true);
        T item = ActivatorFactory.CreateInstance<T>();
        item.setObj(obj);
        return item;
    }

    public static T BindPageNoClone<T>(T instance, Transform trans, string name) where T : AItemPageBase
    {
        if (null == trans)
            return null;
        var findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        var obj = findTrans.gameObject;
        instance ??= ActivatorFactory.CreateInstance<T>();
        instance.setObj(obj);
        return instance;
    }

    public static void CreateItemNoClone<T>(ref T item, RectTransform trans, string name) where T : AItemBase
    {
        if (null == trans) return;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return;
        GameObject obj = findTrans.gameObject;
        //obj.SetActive(true);
        if (item == null) item = ActivatorFactory.CreateInstance<T>();
        else item.Clear();
        item.setObj(obj);
    }

    public static void CreateItemNoClone<T>(ref T item, Transform trans, string name) where T : AItemBase
    {
        if (null == trans) return;
        Transform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return;
        GameObject obj = findTrans.gameObject;
        //obj.SetActive(true);
        if (item == null) item = ActivatorFactory.CreateInstance<T>();
        else item.Clear();
        item.setObj(obj);
    }

    public static T CreatePage<T>(RectTransform trans, string name) where T : AItemPageBase
    {
        if (null == trans)
            return null;

        RectTransform findTrans = Control(name, trans);

        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        GameObject obj = findTrans.gameObject;
        GameObject curObj = GameObject.Instantiate<GameObject>(obj);
        //curObj.SetActive(true);
        Attach(trans, curObj, true);
        T item = ActivatorFactory.CreateInstance<T>();
        item.setObj(curObj);
        return item;
    }

    public static T CreatePage<T>(GameObject templete, RectTransform root) where T : AItemPageBase
    {
        if (null == templete)
            return null;
        GameObject curObj = GameObject.Instantiate<GameObject>(templete, root);
        T item = ActivatorFactory.CreateInstance<T>();
        item.setObj(curObj);
        return item;
    }
    
    public static T CreatePage<T>(Transform trans, string name) where T : AItemPageBase
    {
        if (null == trans)
            return null;

        Transform findTrans = Control(name, trans);

        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        GameObject obj = findTrans.gameObject;
        GameObject curObj = GameObject.Instantiate<GameObject>(obj);
        //curObj.SetActive(true);
        Attach(trans, curObj, true);
        T item = ActivatorFactory.CreateInstance<T>();
        item.setObj(curObj);
        return item;
    }

    public static T CreatePage<T>(GameObject templete, Transform root) where T : AItemPageBase
    {
        if (null == templete)
            return null;
        GameObject curObj = GameObject.Instantiate<GameObject>(templete, root);
        T item = ActivatorFactory.CreateInstance<T>();
        item.setObj(curObj);
        return item;
    }
    #endregion

    #region 坐标转换
    /// <summary> 屏幕坐标到世界坐标 </summary>
    public static Vector3 GetScreenToWorld(RectTransform ract, PointerEventData eventData)
    {
        Vector3 pos = Vector3.zero;
        if (ract == null || eventData == null)
        {
            Debuger.LogError("设定数据为空！");
            return pos;
        }
        RectTransformUtility.ScreenPointToWorldPointInRectangle(ract, eventData.position, eventData.pressEventCamera, out pos);
        return pos;
    }

    /// <summary> 世界坐标到屏幕坐标 如果Canvas是Screen Space-overlay模式，cam参数为null </summary>
    public static Vector2 GetWordToCanvasPos(Canvas canvas, Vector3 worldPos, Camera cam = null)
    {
        Vector2 position = Vector2.zero;
        if (worldPos == null || canvas == null)
        {
            Debuger.LogError("设定数据为空！");
            return position;
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, worldPos, cam, out position);
        return position;
    }

    /// <summary> 拖拽跟随 </summary>
    public static void DragFollow(RectTransform trans, PointerEventData eventData)
    {
        if (trans == null || eventData == null)
        {
            Debuger.LogError("设定数据为空！");
            return;
        }
        trans.position = GetScreenToWorld(trans, eventData);
    }
    #endregion
}

