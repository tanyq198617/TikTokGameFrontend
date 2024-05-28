using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICompleted : IUpdate
{
    bool IsCompleted();

    void Destory();

    void PutBackObj();
}

public class ListMgrT<T> where T : ICompleted
{
    public List<T> m_list = new List<T>();
    public int Count { get { return m_list.Count; } }

    public void Init()
    {
        m_list.Clear();
    }

    public void Add(T obj)
    {
        if (obj == null) return;
        m_list.Add(obj);
    }

    public bool Contains(T obj) { return m_list.Contains(obj); }

    public void Remove(T obj)
    {
        if (obj == null) return;
        if (m_list.Contains(obj))
        {
            m_list.Remove(obj);
            obj.PutBackObj();
        }
    }

    public void RemoveNoPutBack(T obj)
    {
        if (obj == null) return;
        if (m_list.Contains(obj))
        {
            m_list.Remove(obj);
        }
    }

    public void Clear()
    {
        m_list.Clear();
    }

    public void Destory()
    {
        if (m_list.Count != 0)
        {
            for (int i = m_list.Count - 1; i >= 0; i--)
            {
                if (m_list[i] != null) m_list[i].Destory();
            }
        }
        m_list.Clear();
    }

    public void PutBackObj()
    {
        if (m_list.Count != 0)
        {
            for (int i = m_list.Count - 1; i >= 0; i--)
            {
                if (m_list[i] != null) m_list[i].PutBackObj();
            }

        }
        m_list.Clear();
    }

    private T _temp = default(T);
    public void Update()
    {
        if (m_list.Count != 0)
        {
            for (int i = m_list.Count - 1; i >= 0; i--)
            {
                _temp = m_list[i];
                if (_temp == null) m_list.RemoveAt(i);
                else
                {
                    _temp.Update();
                    if (_temp.IsCompleted())
                    {
                        m_list.RemoveAt(i);
                        _temp.PutBackObj();
                    }
                }
            }
        }
    }
}


