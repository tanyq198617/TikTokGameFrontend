using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI工具集
/// </summary>
internal class UIUtility
{

    #region 检测组件是否存在
    private static bool CheckComponent(GameObject obj, string parent, string name)
    {
        if (null == obj)
        {
            Debug.LogWarning("!!预件" + parent + "下未找到" + name);
            return false;
        }
        return true;
    }

    private static bool CheckComponent(RectTransform trans, string parent, string name)
    {
        if (null == trans)
        {
            Debug.LogWarning("!!预件" + parent + "下未找到" + name);
            return false;
        }
        return true;
    }

    private static bool CheckComponent(Transform trans, string parent, string name)
    {
        if (null == trans)
        {
            Debug.LogWarning("!!预件" + parent + "下未找到" + name);
            return false;
        }
        return true;
    }

    private static bool IsExist(UnityEngine.GameObject comp)
    {
        if (comp == null)
        {
            Debug.LogWarning("!!预件不存在!!!");
        }
        return comp != null;
    }
    private static bool IsExist(UnityEngine.Component comp)
    {
        if (comp == null)
        {
            Debug.LogWarning("!!预件不存在!!!");
        }
        return comp != null;
    }
    #endregion

    #region 获取控件
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
            if (ctrans != null && ctrans.name.Equals(name))
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
            if (ctrans != null && ctrans.name.Equals(name))
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


    public static T Control<T>(string name, RectTransform gameObj) where T : Component
    {
        if (null == gameObj || gameObj.childCount == 0)
            return null;

        for (int i = 0; i < gameObj.childCount; ++i)
        {
            T ctrans = gameObj.GetChild(i).GetComponent<T>();
            if (ctrans != null && ctrans.name.Equals(name))
                return ctrans;
        }

        for (int i = 0; i < gameObj.childCount; ++i)
        {
            T ttTrans = Control<T>(name, gameObj.GetChild(i).GetComponent<RectTransform>());
            if (ttTrans != null)
                return ttTrans;
        }
        return null;
    }

    public static T Control<T>(string name, Transform gameObj) where T : Component
    {
        if (null == gameObj || gameObj.childCount == 0)
            return null;

        for (int i = 0; i < gameObj.childCount; ++i)
        {
            T ctrans = gameObj.GetChild(i).GetComponent<T>();
            if (ctrans!= null && ctrans.name.Equals(name))
                return ctrans;
        }

        for (int i = 0; i < gameObj.childCount; ++i)
        {
            T ttTrans = Control<T>(name, gameObj.GetChild(i).GetComponent<Transform>());
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
            Debug.LogError($"查找不到游戏物体，{gameObj.name}下的{name}");
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
            Debug.LogError($"设定{ui.ToString()}数据为空！");
            return;
        }
        else if (ui is Image)
        {
            if (text == null && reset)
                (ui as Image).sprite = null;
            else
                (ui as Image).sprite = text as Sprite;
        }
        else if (ui is Text)
        {
            if (text == null && reset)
                (ui as Text).text = null;
            else
                (ui as Text).text = text.ToString();
        }
        else if (ui is TMPro.TextMeshProUGUI)
        {
            if (text == null && reset)
                (ui as TMPro.TextMeshProUGUI).text = null;
            else
                (ui as TMPro.TextMeshProUGUI).text = text.ToString();
        }
        else if (ui is TMPro.TMP_InputField)
        {
            if (text == null && reset)
                (ui as TMPro.TMP_InputField).text = "";
            else
                (ui as TMPro.TMP_InputField).text = text.ToString();
        }
        else if (ui is RawImage)
        {
            RawImage uiTex = ui as RawImage;
            if (uiTex == null) return;

            Texture tex = text as Texture;
            if (tex != null)
            {
                uiTex.texture = tex;
                //uiTex.texture.width = tex.width;
                //uiTex.texture.height = tex.height;
            }
        }
    }

    public static void Safe_Float<T>(ref T ui, float value)
    {
        if (ui == null) return;

        if (ui is Slider)
            (ui as Slider).value = value;
        else if (ui is Scrollbar)
            (ui as Scrollbar).value = value;
    }
    #endregion

    #region 绑定事件

    #region 点击事件
    public static GameObject BindClickEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call, int se_sound = 0)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindClickEvent(findTrans.gameObject, call, se_sound);
    }

    public static GameObject BindClickEvent(GameObject obj, UIEventHandle<PointerEventData> call, int se_sound = 0)
    {
        if (null == obj) return null;
        call += (o, ev) => {  };
        EventTriggerListener.Get(obj).onClick.AddListener(call);
        return obj;
    }
    #endregion

    #region 双击事件
    public static GameObject BindDoubleClickEvent(RectTransform trans, string name, UIEventHandle<PointerEventData> call, int se_sound = 0)
    {
        if (null == trans)
            return null;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return null;
        return BindDoubleClickEvent(findTrans.gameObject, call, se_sound);
    }

    public static GameObject BindDoubleClickEvent(GameObject obj, UIEventHandle<PointerEventData> call, int se_sound = 0)
    {
        if (null == obj) return null;
        call += (o, ev) => {  };
        EventTriggerListener.Get(obj).onDoubleClick.AddListener(call);
        return obj;
    }
    #endregion

    #region 监听值改变


    public static void BindValueChanged<T>(ref T ui, Action<object> callback)
    {
        if (ui == null) return;
        if (callback == null)
        {
            Debug.LogError($"设定{ui.ToString()}数据为空！");
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

    #region 绑定Button点击事件
    public static Button BindButtonClick(RectTransform trans, string name, UnityAction call, int se_sound = 0)
    {
        Button button = GetComponent<Button>(trans, name);
        if (button == null)
            return null;
        button.onClick.AddListener(call);
        return button;
    }

    public static Button BindButtonClick(GameObject obj, UnityAction call, int se_sound = 0)
    {
        Button button = obj.GetComponent<Button>();
        if (button == null)
            return null;
        button.onClick.AddListener(call);
        return button;
    }

    public static Button BindButtonClick<T>(RectTransform trans, string name, UnityAction<T> call, T param, int se_sound = 0)
    {
        Button button = GetComponent<Button>(trans, name);
        if (button == null)
            return null;
        button.onClick.AddListener(delegate
        {
            call(param);
        });
        return button;
    }

    public static Button BindButtonClick<T>(GameObject obj, UnityAction<T> call, T param, int se_sound = 0)
    {
        Button button = obj.GetComponent<Button>();
        if (button == null)
            return null;
        button.onClick.AddListener(delegate
        {
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
        curObj.SetActive(true);
        Attach(parent, curObj, isIncludeScale);
        T item = Activator.CreateInstance<T>();
        item.setObj(curObj);
        return item;
    }

    public static T CreateItem<T>(GameObject obj, Transform parent) where T : AItemBase
    {
        if (obj == null) return null;

        GameObject curObj = GameObject.Instantiate<GameObject>(obj);
        curObj.SetActive(true);
        Attach(parent, curObj, true);
        T item = Activator.CreateInstance<T>();
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
        obj.SetActive(true);
        T item = Activator.CreateInstance<T>();
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
        obj.SetActive(true);
        T item = Activator.CreateInstance<T>();
        item.setObj(obj);
        return item;
    }

    public static void CreateItemNoClone<T>(ref T item, RectTransform trans, string name) where T : AItemBase
    {
        if (null == trans) return;
        RectTransform findTrans = Control(name, trans);
        if (!CheckComponent(findTrans, trans.name, name))
            return;
        GameObject obj = findTrans.gameObject;
        obj.SetActive(true);
        if (item == null) item = Activator.CreateInstance<T>();
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
        obj.SetActive(true);
        if (item == null) item = Activator.CreateInstance<T>();
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
        curObj.SetActive(true);
        Attach(trans, curObj, true);
        T item = Activator.CreateInstance<T>();
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
            Debug.LogError("设定数据为空！");
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
            Debug.LogError("设定数据为空！");
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
            Debug.LogError("设定数据为空！");
            return;
        }
        trans.position = GetScreenToWorld(trans, eventData);
    }
    #endregion        

        
}

internal static class UIUtilityEx
{
    #region 拓展
    public static void SetActiveEX(this Transform trans, bool active)
    {
        if (trans == null) { return; }
        trans.gameObject.SetActiveEX(active);
    }
    public static void SetActiveEX(this MonoBehaviour mono, bool active)
    {
        if (mono == null) { return; }
        mono.gameObject.SetActiveEX(active);
    }
    public static void SetActiveEX(this GameObject obj, bool active)
    {
        if (obj == null)
        {
            Debug.LogError("游戏物体不存在！！！");
            return;
        }
        if (active)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
            }
        }
    }
    #endregion
}
