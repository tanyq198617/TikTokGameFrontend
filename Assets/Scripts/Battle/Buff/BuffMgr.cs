using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMgr
{
    /// <summary> buff集合, key=buffID </summary>
    private static readonly Dictionary<int, BuffHandler> redBuffDict = new Dictionary<int, BuffHandler>();
    
    /// <summary> buff集合, key=buffID </summary>
    private static readonly Dictionary<int, BuffHandler> blueBuffDict = new Dictionary<int, BuffHandler>();

    /// <summary> buff集合, key=buffID </summary>
    private static readonly List<BuffHandler> handlers = new List<BuffHandler>();

    //临时地址
    private static BuffHandler tmp;

    public static BuffHandler AddBuff(CampType campType, int buffId)
    {
        BuffHandler handler = null;
        if (campType == CampType.红)
        {
            if (!redBuffDict.TryGetValue(buffId, out handler))
            {
                handler = BuffHandler.Create(campType, buffId);
                redBuffDict.Add(buffId, handler);
                handlers.Add(handler);
            }
            redBuffDict[buffId].AddBuff();
        }
        else
        {
            if (!blueBuffDict.TryGetValue(buffId, out handler))
            {
                handler = BuffHandler.Create(campType, buffId);
                blueBuffDict.Add(buffId, handler);
                handlers.Add(handler);
            }
            blueBuffDict[buffId].AddBuff();
        }
        
        EventMgr.Dispatch(BattleEvent.Battle_BUFF_Changed, campType, handler);
        return handler;
    }

    public static BuffHandler GetBuff(CampType campType, int buffId)
    {
        BuffHandler handler = null;
        if (campType == CampType.红)
            redBuffDict.TryGetValue(buffId, out handler);
        else 
            blueBuffDict.TryGetValue(buffId, out handler);
        return handler;
    }

    /// <summary> 判断阵营是否含有buff /// </summary>
    public static bool IsHaveBuff(CampType campType)
    {
        if (campType == CampType.红)
            return redBuffDict.Count > 0;
        else if (campType == CampType.蓝)
            return blueBuffDict.Count > 0;
        else
            return false;  
    }
    
    public static void Update()
    {
        for (int i = handlers.Count - 1; i >= 0; i--)
        {
            tmp = handlers[i];
            if (tmp == null)
                handlers.RemoveAt(i);
            else
            {
                if (tmp.IsComplete())
                {
                    if (tmp.CampType == CampType.红)
                        redBuffDict.Remove(tmp.BuffID);
                    else 
                        blueBuffDict.Remove(tmp.BuffID);
                    handlers.RemoveAt(i);
                    BallFactory.OnBuffChanged(tmp.CampType, tmp);
                    EventMgr.Dispatch(BattleEvent.Battle_BUFF_Changed, tmp.CampType, tmp);
                }
                else
                {
                    tmp.Update();
                }
            }
        }
    }

    public static void Release()
    {
        foreach (var kv in redBuffDict)
            kv.Value.Clear();
        foreach (var kv in blueBuffDict)
            kv.Value.Clear();
        redBuffDict.Clear();
        blueBuffDict.Clear();
        handlers.Clear();
        tmp = null;
    }
}
