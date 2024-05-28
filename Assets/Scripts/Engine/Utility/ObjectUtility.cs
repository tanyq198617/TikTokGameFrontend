using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public static class ObjectUtility
{
    public static void SetMaterial<T>(Transform trans, Material mat) where T : Renderer
    {
        if (trans == null) return;
        T render = trans.GetComponent<T>();
        if (render != null)
            render.material = mat;
    }

    public static void SetMaterialColor(MeshRenderer render, Color color)
    {
        if (render == null) return;
        render.material.SetColor("_TintColor", color);
    }

    public static T Safe_CreateObj<T>(ref GameObject obj, string name, bool isDontDestory = false) where T : Component
    {
        if (obj == null)
        {
            obj = new GameObject(name);
            if (isDontDestory) GameObject.DontDestroyOnLoad(obj);
        }
        return obj.AddComponent<T>();
    }

    public static void Safe_CreateObj(ref GameObject obj, string name, bool isDontDestory = false)
    {
        if (obj == null)
        {
            obj = new GameObject(name);
            if (isDontDestory) GameObject.DontDestroyOnLoad(obj);
        }
    }

    public static void Reset(this GameObject obj)
    {
        if (obj == null)
            return;
        Reset(obj.transform);
    }
    public static void Reset(this Transform trans)
    {
        if (trans == null)
            return;
        trans.localPosition = Vector3.zero;
        trans.localScale = Vector3.one;
        trans.localRotation = Quaternion.identity;
    }

    public static RectTransform rectTransform(this Component cp)
    {
        if (cp == null) return null;
        return cp.transform as RectTransform;
    }

    public static RectTransform rectTransform(this GameObject cp)
    {
        if (cp == null) return null;
        return cp.GetComponent<RectTransform>();
    }

    public static bool Safe_ActiveObj(GameObject obj, bool isActive)
    {
        if (obj == null)
            return false;
        if (obj.activeSelf == isActive)
            return false;
        obj.SetActive(isActive);
        return true;
    }


    public static void Safe_ActiveTrans(Transform trans, bool isActive)
    {
        if (trans != null) Safe_ActiveObj(trans.gameObject, isActive);
    }


    public static void Safe_HideObj(ref GameObject obj, bool isClear = true)
    {
        if (obj != null)
        {
            obj.SetActive(false);
            if (isClear) obj = null;
        }
    }

    public static void Safe_Action<T>(ref T obj, Action<T> CallBack)
    {
        if (obj == null) return;
        if (CallBack != null) CallBack(obj);
    }

    public static void Safe_Destory<T>(ref T obj, bool isClear = true) where T : Object
    {
        if (obj != null)
        {
            Object.Destroy(obj);
            if (isClear) obj = null;
        }
    }

    public static void Safe_DestoryDic<T>(ref Dictionary<string, T> dic, bool isClear = true) where T : UnityEngine.Object
    {
        if (dic == null || dic.Count == 0) return;
        List<string> curList = new List<string>(dic.Keys);
        for (int i = 0; i < curList.Count; i++)
        {
            if (dic[curList[i]] != null)
                UnityEngine.Object.Destroy(dic[curList[i]]);
        }
        dic.Clear();
        if (isClear) dic = null;
    }

    public static void Safe_DestoryObjList<T>(ref List<T> list, bool isClear = true) where T : UnityEngine.Object
    {
        if (list == null) return;
        if (list.Count != 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                    UnityEngine.Object.Destroy(list[i]);
            }
            list.Clear();
        }
        if (isClear) list = null;
    }

    public static void Safe_ClearDic<T>(ref Dictionary<string, T> dic, bool isClear = false) where T : UnityEngine.Object
    {
        if (dic == null) return;
        dic.Clear();
        if (isClear) dic = null;
    }

    public static void Safe_ClearDic2<T>(ref Dictionary<string, T[]> dic, bool isClear = false) where T : class
    {
        if (dic == null) return;
        dic.Clear();
        if (isClear) dic = null;
    }

    public static void Safe_ClearList<T>(ref List<T> list, bool isClear = false)
    {
        if (list == null) return;
        list.Clear();
        if (isClear) list = null;
    }

    public static void Safe_ClearDic<T>(Dictionary<string, T> dic)
    {
        if (dic == null) return;
        dic.Clear();
    }

    public static void Safe_CallBack(Action callBack)
    {
        if (callBack != null)
        {
            callBack();
        }
    }

    public static void Safe_CallBack<T>(ref Action<T> callBack, T data, bool isClear)
    {
        if (callBack != null)
        {
            callBack(data);
            if (isClear) callBack = null;
        }
    }

    public static void Safe_CallBack<T, U>(ref Action<T, U> callBack, T t, U u, bool isClear)
    {
        if (callBack != null)
        {
            callBack(t, u);
            if (isClear) callBack = null;
        }
    }

    public static U Safe_Func<T, U>(ref Func<T, U> callBack, T data, bool isClear)
    {
        if (callBack != null)
        {
            U result = callBack(data);
            if (isClear) callBack = null;
            return result;
        }
        return default(U);
    }

    public static void Safe_CallBack<T>(Action<T> callBack, T data)
    {
        if (callBack != null)
            callBack(data);
    }

    public static void Safe_CallBack(ref Action callBack, bool isClear = false)
    {
        if (callBack != null)
            callBack();
        if (isClear) callBack = null;
    }

    public static void Safe_Enable<T>(T behaviour, bool isEnable) where T : Behaviour
    {
        if (behaviour == null)
            return;

        if (behaviour.enabled != isEnable)
            behaviour.enabled = isEnable;
    }

    public static void Safe_EnableT<T>(T behaviour, bool isEnable) where T : Behaviour
    {
        if (behaviour != null) behaviour.enabled = isEnable;
    }

    public static void Safe_EnableT<T>(GameObject obj, bool isEnable) where T : Behaviour
    {
        if (obj == null) return;
        Safe_EnableT<T>(obj.transform, isEnable);
    }

    public static void Safe_EnableT<T>(Transform trans, bool isEnable) where T : Behaviour
    {
        if (trans == null) return;
        Safe_EnableT(trans.GetComponent<T>(), isEnable);
    }

    public static void Safe_EnableCollider<T>(GameObject obj, bool isEnable) where T : Collider
    {
        if (obj == null) return;
        Safe_EnableCollider<T>(obj.transform, isEnable);
    }

    public static void Safe_EnableCollider<T>(Transform trans, bool isEnable) where T : Collider
    {
        if (trans == null) return;
        T behaviour = trans.GetComponent<T>();
        if (behaviour != null) behaviour.enabled = isEnable;
    }

    public static bool IsNull(this object obj)
    {
        return ReferenceEquals(obj, null);
    }

    public static bool IsNotNull(this object obj)
    {
        return ReferenceEquals(obj, null) == false;
    }

    public static bool CheckType(this object obj, Type type)
    {
        return obj.GetType() == type;
    }

    public static bool IsNotNullOrNoCount<T>(this ICollection<T> objs)
    {
        return !ReferenceEquals(objs, null) && objs.Count != 0;
    }

    public static bool IsNullOrNoCount<T>(this ICollection<T> objs)
    {
        return ReferenceEquals(objs, null) || objs.Count == 0;
    }

    public static Vector3 CheckForward(Vector3 forward, bool isRandom = false)
    {
        if (forward != Vector3.zero) return forward;
        if (!isRandom) return Vector3.forward;
        else
        {
            float random = UnityEngine.Random.Range(0, 360);
            return new Vector3(Mathf.Cos(random), 0, Mathf.Sin(random));
        }
    }

    public static Vector3 CheckForward(Vector3 dec, Vector3 original)
    {
        if (dec != Vector3.zero) return dec;
        else return original;
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
    
    public static void SetEnabledEX(this Behaviour mono, bool active)
    {
        if (mono == null) { return; }

        if (active)
        {
            if (!mono.enabled)
                mono.enabled = true;
        }
        else
        {
            if (mono.enabled)
                mono.enabled = false;
        }
    }
    
    public static void SetEnabledEX(this Collider mono, bool active)
    {
        if (mono == null) { return; }

        if (active)
        {
            if (!mono.enabled)
                mono.enabled = true;
        }
        else
        {
            if (mono.enabled)
                mono.enabled = false;
        }
    }

    public static void ResetUI(this TMPro.TextMeshProUGUI ui)
    {
        if (ui == null) { return; }
        ui.text = "";
    }

    public static void ResetUI(this TMPro.TMP_InputField ui)
    {
        if (ui == null) { return; }
        ui.text = "";
    }

    public static int ToInt(this Enum e)
    {
        return e.GetHashCode();
    }
    public static uint ToUInt(this Enum e)
    {
        return (uint)e.GetHashCode();
    }
    public static string ValueToString(this Enum e)
    {
        return ToInt(e).ToString();
    }

    public static T Find<T>(this T[] array, Func<T, bool> condition) where T : new()
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]))
                return array[i];
        }
        return default;
    }

    public static bool IsContains(this List<Vector3> arr, Vector3 item)
    {
        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i].IsEquals(item))
                return true;
        }
        return false;
    }

    public static bool IsContains<T>(this T[] arr, T item) where T : IComparable<T>
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].CompareTo(item) == 0)
                return true;
        }
        return false;
    }

    public static void Random<T>(this List<T> sources, System.Random rd)
    {
        int index = 0;
        T temp;
        for (int i = 0; i < sources.Count; i++)
        {
            index = rd.Next(0, sources.Count - 1);
            if (index != i)
            {
                temp = sources[i];
                sources[i] = sources[index];
                sources[index] = temp;
            }
        }
    }

    public static System.Threading.Tasks.Task RandomAsync<T>(this List<T> sources, System.Random rd)
    {
        System.Threading.Tasks.TaskCompletionSource<List<T>> tcs = new System.Threading.Tasks.TaskCompletionSource<List<T>>();

        int index = 0;
        T temp;
        for (int i = 0; i < sources.Count; i++)
        {
            index = rd.Next(0, sources.Count - 1);
            if (index != i)
            {
                temp = sources[i];
                sources[i] = sources[index];
                sources[index] = temp;
            }
        }

        tcs.SetResult(sources);
        return tcs.Task;
    }

    public static void Swap<T>(ref T t1, ref T t2)
    {
        T t3 = t2;
        t2 = t1;
        t1 = t3;
    }

    public static string Write<T>(this IList<T> objs, string separator = ",")
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for (int i = 0; i < objs.Count; i++)
        {
            builder.Append(objs[i]);
            if (i < objs.Count - 1)
                builder.Append(separator);
        }
        return builder.ToString();
    }

    public static void Destroy(this UnityEngine.Object obj)
    {
        if (obj == null) return;
        GameObject.Destroy(obj);
        obj = null;
    }

    public static void DestroyImmediate(this UnityEngine.Object obj, bool allowDestroyingAssets = false)
    {
        if (obj == null) return;
        GameObject.DestroyImmediate(obj, allowDestroyingAssets);
        obj = null;
    }

    public static void RemoveComponent<T>(this UnityEngine.Behaviour obj) where T : Behaviour
    {
        if (obj == null || obj.gameObject == null) return;
        var t = obj.gameObject.GetComponent<T>();
        if (t == null) return;
        GameObject.Destroy(t);
    }

    public static void TMP_TextLinkClick(this TMPro.TextMeshProUGUI text, PointerEventData eventData, Action<string, string> OnHandler)
    {
        if (text == null) return;
        Vector3 pos = new Vector3(eventData.position.x, eventData.position.y, 0);
        int linkIndex = TMPro.TMP_TextUtilities.FindIntersectingLink(text, pos, UIRoot.Instance.UICamera);
        if (linkIndex > -1)
        {
            TMPro.TMP_LinkInfo linkInfo = text.textInfo.linkInfo[linkIndex];
            string linkID = linkInfo.GetLinkID();
            string linkText = linkInfo.GetLinkText();
            OnHandler?.Invoke(linkID, linkText);
        }
    }

    public static long DateConvertToUnixTimestamp(this DateTime date)
    {
        var unixTimestamp = (date.ToUniversalTime().Ticks - 621355968000000000) / 10000; //毫秒
        return unixTimestamp;
    }
}
