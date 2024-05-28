using System;
using EventDispatcher;

/// <summary>
/// 全局事件管理器
/// </summary>
public static partial class EventMgr
{
    private static readonly Event _event = new Event();
    
    public static T Get<T>() => _event.GetInterface<T>();
    public static void Register<T>(T instance) => _event.RegWrapInterface<T>(instance);
    
    #region 注册全局监听
    public static bool AddEventListener(int eventType, Action handler) => _event.AddEventListener(eventType, handler);
    public static bool AddEventListener<T1>(int eventType, Action<T1> handler) => _event.AddEventListener(eventType, handler);
    public static bool AddEventListener<T1, T2>(int eventType, Action<T1, T2> handler) => _event.AddEventListener(eventType, handler);
    public static bool AddEventListener<T1, T2, T3>(int eventType, Action<T1, T2, T3> handler) => _event.AddEventListener(eventType, handler);
    public static bool AddEventListener<T1, T2, T3, T4>(int eventType, Action<T1, T2, T3, T4> handler) => _event.AddEventListener(eventType, handler);
    public static bool AddEventListener<T1, T2, T3, T4, T5>(int eventType, Action<T1, T2, T3, T4, T5> handler) => _event.AddEventListener(eventType, handler);
    public static bool AddEventListener<T1, T2, T3, T4, T5, T6>(int eventType, Action<T1, T2, T3, T4, T5, T6> handler) => _event.AddEventListener(eventType, handler);
    #endregion
    
    #region 注册全局监听
    public static bool AddEventListener(string eventType, Action handler) => _event.Dispatcher.AddEventListener(eventType.StringToHash(), handler);
    public static bool AddEventListener<T1>(string eventType, Action<T1> handler) => _event.Dispatcher.AddEventListener(eventType.StringToHash(), handler);
    public static bool AddEventListener<T1, T2>(string eventType, Action<T1, T2> handler) => _event.AddEventListener(eventType.StringToHash(), handler);
    public static bool AddEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler) => _event.AddEventListener(eventType.StringToHash(), handler);
    public static bool AddEventListener<T1, T2, T3, T4>(string eventType, Action<T1, T2, T3, T4> handler) => _event.AddEventListener(eventType.StringToHash(), handler);
    public static bool AddEventListener<T1, T2, T3, T4, T5>(string eventType, Action<T1, T2, T3, T4, T5> handler) => _event.AddEventListener(eventType.StringToHash(), handler);
    public static bool AddEventListener<T1, T2, T3, T4, T5, T6>(string eventType, Action<T1, T2, T3, T4, T5, T6> handler) => _event.AddEventListener(eventType.StringToHash(), handler);
    #endregion

    #region 移除全局监听
    public static void RemoveEventListener(int eventType, Action handler) => _event.RemoveEventListener(eventType, handler);
    public static void RemoveEventListener<T1>(int eventType, Action<T1> handler) => _event.RemoveEventListener(eventType, handler);
    public static void RemoveEventListener<T1, T2>(int eventType, Action<T1, T2> handler) => _event.RemoveEventListener(eventType, handler);
    public static void RemoveEventListener<T1, T2, T3>(int eventType, Action<T1, T2, T3> handler) => _event.RemoveEventListener(eventType, handler);
    public static void RemoveEventListener<T1, T2, T3, T4>(int eventType, Action<T1, T2, T3, T4> handler) => _event.RemoveEventListener(eventType, handler);
    public static void RemoveEventListener<T1, T2, T3, T4, T5>(int eventType, Action<T1, T2, T3, T4, T5> handler) => _event.RemoveEventListener(eventType, handler);
    public static void RemoveEventListener<T1, T2, T3, T4, T5, T6>(int eventType, Action<T1, T2, T3, T4, T5, T6> handler) => _event.RemoveEventListener(eventType, handler);
    #endregion
    
    #region 移除全局监听
    public static void RemoveEventListener(string eventType, Action handler) => _event.RemoveEventListener(eventType.StringToHash(), handler);
    public static void RemoveEventListener<T1>(string eventType, Action<T1> handler) => _event.RemoveEventListener(eventType.StringToHash(), handler);
    public static void RemoveEventListener<T1, T2>(string eventType, Action<T1, T2> handler) => _event.RemoveEventListener(eventType.StringToHash(), handler);
    public static void RemoveEventListener<T1, T2, T3>(string eventType, Action<T1, T2, T3> handler) => _event.RemoveEventListener(eventType.StringToHash(), handler);
    public static void RemoveEventListener<T1, T2, T3, T4>(string eventType, Action<T1, T2, T3, T4> handler) => _event.RemoveEventListener(eventType.StringToHash(), handler);
    public static void RemoveEventListener<T1, T2, T3, T4, T5>(string eventType, Action<T1, T2, T3, T4, T5> handler) => _event.RemoveEventListener(eventType.StringToHash(), handler);
    public static void RemoveEventListener<T1, T2, T3, T4, T5, T6>(string eventType, Action<T1, T2, T3, T4, T5, T6> handler) => _event.RemoveEventListener(eventType.StringToHash(), handler);
    #endregion

    #region 消息分发
    public static void Dispatch(int eventType) => _event.Dispatch(eventType);
    public static void Dispatch<T1>(int eventType, T1 arg1) => _event.Dispatch(eventType, arg1);
    public static void Dispatch<T1, T2>(int eventType, T1 arg1, T2 arg2) => _event.Dispatch(eventType, arg1, arg2);
    public static void Dispatch<T1, T2, T3>(int eventType, T1 arg1, T2 arg2, T3 arg3) => _event.Dispatch(eventType, arg1, arg2, arg3);
    public static void Dispatch<T1, T2, T3, T4>(int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _event.Dispatch(eventType, arg1, arg2, arg3, arg4);
    public static void Dispatch<T1, T2, T3, T4, T5>(int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => _event.Dispatch(eventType, arg1, arg2, arg3, arg4, arg5);
    public static void Dispatch<T1, T2, T3, T4, T5, T6>(int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => _event.Dispatch(eventType, arg1, arg2, arg3, arg4, arg5, arg6);
    #endregion

    #region 消息分发
    public static void Dispatch(string eventType) => _event.Dispatch(eventType.StringToHash());
    public static void Dispatch<T1>(string eventType, T1 arg1) => _event.Dispatch(eventType.StringToHash(), arg1);
    public static void Dispatch<T1, T2>(string eventType, T1 arg1, T2 arg2) => _event.Dispatch(eventType.StringToHash(), arg1, arg2);
    public static void Dispatch<T1, T2, T3>(string eventType, T1 arg1, T2 arg2, T3 arg3) => _event.Dispatch(eventType.StringToHash(), arg1, arg2, arg3);
    public static void Dispatch<T1, T2, T3, T4>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => _event.Dispatch(eventType.StringToHash(), arg1, arg2, arg3, arg4);
    public static void Dispatch<T1, T2, T3, T4, T5>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => _event.Dispatch(eventType.StringToHash(), arg1, arg2, arg3, arg4, arg5);
    public static void Dispatch<T1, T2, T3, T4, T5, T6>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => _event.Dispatch(eventType.StringToHash(), arg1, arg2, arg3, arg4, arg5, arg6);
    #endregion
}
