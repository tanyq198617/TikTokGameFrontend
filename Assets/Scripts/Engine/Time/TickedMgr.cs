using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickedMgr : Singleton<TickedMgr>
{
    List<TickedBase> _tickedList = new List<TickedBase>();

    public bool IsPause { set; get; }

    public void Clear()
    {
        IsPause = true;
        _tickedList.Clear();
    }

    public void Destory()
    {
    }

    public void Init()
    {
        IsPause = false;
    }

    public void Add(TickedBase tick) { if (!_tickedList.Contains(tick)) _tickedList.Add(tick); }

    public void Remove(TickedBase tick) { if (_tickedList.Contains(tick)) _tickedList.Remove(tick); }

    public void FixedUpdate()
    {
        if (IsPause) return;

        if (_tickedList.Count != 0)
        {
            for (int i = 0; i < _tickedList.Count; i++)
                _tickedList[i].FixedUpdate();
        }
    }
}

public static class TickedMgrExtension
{
    public static void Kill(this TickedBase tickedBase, bool isClear = false, bool remove = true)
    {
        if (tickedBase == null)
            return;

        if (remove)
            TickedMgr.Instance.Remove(tickedBase);

        tickedBase.Clear();

        if (isClear)
            tickedBase = null;
    }
}
