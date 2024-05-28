using Sirenix.Utilities;
using System;
using System.Reflection;
using System.Threading.Tasks;

public partial class UIRegister
{
    private static bool IsInit = false;
    public static bool IsCompleted => IsInit;

    public static void Initialize()
    {
        IsInit = false;

        //初始化注册
        _ = Task.Run(AutoRegister);

        //初始化节点
        UIRoot.Instance.OnHotfixInitialize();
    }

    public static void AutoRegister()
    {
        var assembly = typeof(APanelBase).Assembly;
        var types = assembly.FindAllClass(OnSelctUI);
        foreach (var type in types)
        {
            string uiname = type.GetAttribute<UIBindAttribute>().UIName;
            var instance = type.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
            var instanceValue = (APanelBase)instance.GetValue(null);
            //Debug.Log($"[UI注册] 找到类型：{type.FullName}, BindName={uiname}, instanceValue={instanceValue}");
            UIMgr.Instance.Register(uiname, instanceValue);
        }
        IsInit = true;
    }

    private static bool OnSelctUI(Type type)
    {
        if (type == null) return false;
        var cls = type.GetCustomAttributes(typeof(UIBindAttribute), false);
        if (cls == null || cls.Length <= 0) return false;
        return true;
    }
}
