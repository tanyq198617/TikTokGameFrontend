using System;

namespace EventDispatcher
{
    public static class EventExtension
    {
        #region 注册全局监听

        public static bool AddEventListener(this Event @event, int eventType, Action handler) => @event.Dispatcher.AddEventListener(eventType, handler);
        public static bool AddEventListener<T1>(this Event @event, int eventType, Action<T1> handler) => @event.Dispatcher.AddEventListener(eventType, handler);
        public static bool AddEventListener<T1, T2>(this Event @event, int eventType, Action<T1, T2> handler) => @event.Dispatcher.AddEventListener(eventType, handler);
        public static bool AddEventListener<T1, T2, T3>(this Event @event, int eventType, Action<T1, T2, T3> handler) => @event.Dispatcher.AddEventListener(eventType, handler);
        public static bool AddEventListener<T1, T2, T3, T4>(this Event @event, int eventType, Action<T1, T2, T3, T4> handler) => @event.Dispatcher.AddEventListener(eventType, handler);
        public static bool AddEventListener<T1, T2, T3, T4, T5>(this Event @event, int eventType, Action<T1, T2, T3, T4, T5> handler) => @event.Dispatcher.AddEventListener(eventType, handler);
        public static bool AddEventListener<T1, T2, T3, T4, T5, T6>(this Event @event, int eventType, Action<T1, T2, T3, T4, T5, T6> handler) => @event.Dispatcher.AddEventListener(eventType, handler);

        #endregion
        
        
        #region 移除全局监听
        public static void RemoveEventListener(this Event @event, int eventType, Action handler) => @event.Dispatcher.RemoveEventListener(eventType, handler);
        public static void RemoveEventListener<T1>(this Event @event, int eventType, Action<T1> handler) => @event.Dispatcher.RemoveEventListener(eventType, handler);
        public static void RemoveEventListener<T1, T2>(this Event @event, int eventType, Action<T1, T2> handler) => @event.Dispatcher.RemoveEventListener(eventType, handler);
        public static void RemoveEventListener<T1, T2, T3>(this Event @event, int eventType, Action<T1, T2, T3> handler) => @event.Dispatcher.RemoveEventListener(eventType, handler);
        public static void RemoveEventListener<T1, T2, T3, T4>(this Event @event, int eventType, Action<T1, T2, T3, T4> handler) => @event.Dispatcher.RemoveEventListener(eventType, handler);
        public static void RemoveEventListener<T1, T2, T3, T4, T5>(this Event @event, int eventType, Action<T1, T2, T3, T4, T5> handler) => @event.Dispatcher.RemoveEventListener(eventType, handler);
        public static void RemoveEventListener<T1, T2, T3, T4, T5, T6>(this Event @event, int eventType, Action<T1, T2, T3, T4, T5, T6> handler) => @event.Dispatcher.RemoveEventListener(eventType, handler);
        #endregion


        #region 消息分发
        public static void Dispatch(this Event @event, int eventType) => @event.Dispatcher.Send(eventType);
        public static void Dispatch<T1>(this Event @event, int eventType, T1 arg1) => @event.Dispatcher.Send(eventType, arg1);
        public static void Dispatch<T1, T2>(this Event @event, int eventType, T1 arg1, T2 arg2) => @event.Dispatcher.Send(eventType, arg1, arg2);
        public static void Dispatch<T1, T2, T3>(this Event @event, int eventType, T1 arg1, T2 arg2, T3 arg3) => @event.Dispatcher.Send(eventType, arg1, arg2, arg3);
        public static void Dispatch<T1, T2, T3, T4>(this Event @event, int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => @event.Dispatcher.Send(eventType, arg1, arg2, arg3, arg4);
        public static void Dispatch<T1, T2, T3, T4, T5>(this Event @event, int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => @event.Dispatcher.Send(eventType, arg1, arg2, arg3, arg4, arg5);
        public static void Dispatch<T1, T2, T3, T4, T5, T6>(this Event @event, int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => @event.Dispatcher.Send(eventType, arg1, arg2, arg3, arg4, arg5, arg6);
        #endregion
        
        
    }
}