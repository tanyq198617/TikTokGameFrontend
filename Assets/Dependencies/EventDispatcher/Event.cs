using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventDispatcher
{
    internal class EventEntryData
    {
        public object InterfaceWrap;
    }

    public sealed class Event
    {
        private readonly Dictionary<string, EventEntryData> _entryDict = new Dictionary<string, EventEntryData>();

        internal readonly EventDispatcher Dispatcher = new EventDispatcher();
        
        /// <summary>
        /// 事件管理器获取接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetInterface<T>()
        {
            string typeName = typeof(T).FullName;
            if (typeName != null && _entryDict.TryGetValue(typeName, out var entry))
            {
                return (T)entry.InterfaceWrap;
            }
            
            return default(T);
        }

        /// <summary>
        /// 注册wrap函数
        /// </summary>
        /// <param name="callerWrap"></param>
        /// <typeparam name="T"></typeparam>
        public void RegWrapInterface<T>(T callerWrap)
        {
            string typeName = typeof(T).FullName;
            var entry = new EventEntryData
            {
                InterfaceWrap = callerWrap
            };
            if(typeName != null)
                _entryDict.Add(typeName, entry);
        }
    }
}