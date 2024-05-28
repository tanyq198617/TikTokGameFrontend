using GameNetwork;
using System;
using System.Collections.Generic;
using System.Reflection;
using Sproto;

/// <summary>
/// 所有网络消息注册
/// </summary>
public class MsgRegister
{
    private static readonly Dictionary<Type, ATcpHandler> handles = new Dictionary<Type, ATcpHandler>();
    
    
    /// <summary> 通过继承的方式获取 </summary>
    public static void OnRegister()
    {
        var t = typeof(ATcpHandler);
        var assembly = typeof(MsgRegister).Assembly;
        var types = assembly.GetAllSubclass(t);
        foreach (var type in types)
            Register(type);
    }
    
    private static void Register(Type type)
    {
        var t = Activator.CreateInstance(type) as ATcpHandler;
        t.OnRegister();
        handles.Add(type, t);
    }
    
    public static T GetHandle<T>() where T : ATcpHandler
    {
        var type = typeof(T);

        if (handles.TryGetValue(type, out var t))
        {
            return t as T;
        }
        return null;
    }
}
