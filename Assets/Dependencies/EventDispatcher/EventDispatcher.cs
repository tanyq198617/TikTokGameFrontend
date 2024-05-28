using System;
using System.Collections.Generic;

namespace EventDispatcher
{
    internal sealed class EventDispatcher
    {
        private static readonly Dictionary<int, EventDelegateData> _eventTable = new Dictionary<int, EventDelegateData>();

        /// <summary>
        /// 增加事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool AddEventListener(int eventType, Delegate handler)
        {
            if (!_eventTable.TryGetValue(eventType, out var data))
            {
                data = new EventDelegateData(eventType);
                _eventTable.Add(eventType, data);
            }

            return data.AddHandler(handler);
        }

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handler"></param>
        public void RemoveEventListener(int eventType, Delegate handler)
        {
            if (_eventTable.TryGetValue(eventType, out var data))
            {
                data.RemoveHandler(handler);
            }
        }

        #region 事件派发

        public void Send(int eventType)
        {
            if (_eventTable.TryGetValue(eventType, out var data))
            {
                data.CallBack();
            }
        }
        
        public void Send<T1>(int eventType, T1 arg1)
        {
            if (_eventTable.TryGetValue(eventType, out var data))
            {
                data.CallBack(arg1);
            }
        }
        
        public void Send<T1, T2>(int eventType, T1 arg1, T2 arg2)
        {
            if (_eventTable.TryGetValue(eventType, out var data))
            {
                data.CallBack(arg1, arg2);
            }
        }
        
        public void Send<T1, T2, T3>(int eventType, T1 arg1, T2 arg2, T3 arg3)
        {
            if (_eventTable.TryGetValue(eventType, out var data))
            {
                data.CallBack(arg1, arg2, arg3);
            }
        }
        
        public void Send<T1, T2, T3, T4>(int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (_eventTable.TryGetValue(eventType, out var data))
            {
                data.CallBack(arg1, arg2, arg3, arg4);
            }
        }
        
        public void Send<T1, T2, T3, T4, T5>(int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (_eventTable.TryGetValue(eventType, out var data))
            {
                data.CallBack(arg1, arg2, arg3, arg4, arg5);
            }
        }
        
        public void Send<T1, T2, T3, T4, T5, T6>(int eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (_eventTable.TryGetValue(eventType, out var data))
            {
                data.CallBack(arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }
        
        #endregion

    }
}