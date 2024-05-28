using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EventDispatcher
{
    internal sealed class EventDelegateData
    {
        private readonly int m_eventType = 0;
        private readonly List<Delegate> m_listExist = new List<Delegate>();
        private readonly List<Delegate> m_addList = new List<Delegate>();
        private readonly List<Delegate> m_deleteList = new List<Delegate>();
 
        private bool m_isExcute = false;
        private bool m_dirty = false;

        private readonly object[] param1 = new object[1];
        private readonly object[] param2 = new object[2];
        private readonly object[] param3 = new object[3];
        private readonly object[] param4 = new object[4];
        private readonly object[] param5 = new object[5];
        private readonly object[] param6 = new object[6];

        public EventDelegateData(int eventType)
        {
            m_eventType = eventType;
        }

        /// <summary>
        /// 添加注册委托
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool AddHandler(Delegate handler)
        {
            if (m_listExist.Contains(handler))
            {
                Debug.LogError($"重复添加相同事件:{handler.ToString()}");
                return false;
            }

            if (m_isExcute)
            {
                m_dirty = true;
                m_addList.Add(handler);
            }
            else
            {
                m_listExist.Add(handler);
            }

            return true;
        }

        /// <summary>
        /// 移除注册委托
        /// </summary>
        /// <param name="handler"></param>
        public void RemoveHandler(Delegate handler)
        {
            if (m_isExcute)
            {
                m_dirty = true;
                m_deleteList.Add(handler);
            }
            else
            {
                if (!m_listExist.Remove(handler))
                {
                    Debug.LogError($"事件删除失败,不存在的注册:EventId: {m_eventType.HashToString()}");
                }
            }
        }

        /// <summary>
        /// 检查脏数据修正
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckModify()
        {
            m_isExcute = false;
            if (m_dirty)
            {
                int count = m_addList.Count;
                for (int i = 0; i < count; i++)
                {
                    m_listExist.Add(m_addList[i]);
                }
                m_addList.Clear();

                count = m_deleteList.Count;
                for (int i = 0; i < count; i++)
                {
                    m_listExist.Remove(m_deleteList[i]);
                }
                m_deleteList.Clear();
            }
        }
        
        public void CallBack()
        {
            m_isExcute = true;
            int count = m_listExist.Count;
            for (int i = 0; i < count; i++)
            {
                var d = m_listExist[i];
                if (d is Action action)
                {
                    action();
                }
            }

            CheckModify();
        }

        public void CallBack<T1>(T1 arg1)
        {
            m_isExcute = true;
            int count = m_listExist.Count;
            for (int i = 0; i < count; i++)
            {
                var d = m_listExist[i];
                if (d is Action<T1> action)
                {
                    action(arg1);
                }
                else
                {
                    try
                    {
                        param1[0] = arg1;
                        d.Method.Invoke(d.Target, param1);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            CheckModify();
        }
        
        public void CallBack<T1, T2>(T1 arg1, T2 arg2)
        {
            m_isExcute = true;
            int count = m_listExist.Count;
            for (int i = 0; i < count; i++)
            {
                var d = m_listExist[i];
                if (d is Action<T1, T2> action)
                {
                    action(arg1, arg2);
                }
                else
                {
                    try
                    {
                        param2[0] = arg1;
                        param2[1] = arg2;
                        d.Method.Invoke(d.Target, param2);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            CheckModify();
        }      
        
        public void CallBack<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            m_isExcute = true;
            int count = m_listExist.Count;
            for (int i = 0; i < count; i++)
            {
                var d = m_listExist[i];
                if (d is Action<T1, T2, T3> action)
                {
                    action(arg1, arg2, arg3);
                }
                else
                {
                    try
                    {
                        param3[0] = arg1;
                        param3[1] = arg2;
                        param3[2] = arg3;
                        d.Method.Invoke(d.Target, param3);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            CheckModify();
        }
        
        public void CallBack<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            m_isExcute = true;
            int count = m_listExist.Count;
            for (int i = 0; i < count; i++)
            {
                var d = m_listExist[i];
                if (d is Action<T1, T2, T3, T4> action)
                {
                    action(arg1, arg2, arg3, arg4);
                }
                else
                {
                    try
                    {
                        param4[0] = arg1;
                        param4[1] = arg2;
                        param4[2] = arg3;
                        param4[3] = arg4;
                        d.Method.Invoke(d.Target, param4);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            CheckModify();
        }
        
        public void CallBack<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            m_isExcute = true;
            int count = m_listExist.Count;
            for (int i = 0; i < count; i++)
            {
                var d = m_listExist[i];
                if (d is Action<T1, T2, T3, T4, T5> action)
                {
                    action(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    try
                    {
                        param5[0] = arg1;
                        param5[1] = arg2;
                        param5[2] = arg3;
                        param5[3] = arg4;
                        param5[4] = arg5;
                        d.Method.Invoke(d.Target, param5);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            CheckModify();
        }
        
        public void CallBack<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            m_isExcute = true;
            int count = m_listExist.Count;
            for (int i = 0; i < count; i++)
            {
                var d = m_listExist[i];
                if (d is Action<T1, T2, T3, T4, T5, T6> action)
                {
                    action(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                else
                {
                    try
                    {
                        param6[0] = arg1;
                        param6[1] = arg2;
                        param6[2] = arg3;
                        param6[3] = arg4;
                        param6[4] = arg5;
                        param6[5] = arg6;
                        d.Method.Invoke(d.Target, param6);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            CheckModify();
        }
    }
}