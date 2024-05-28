using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局关卡数据
/// </summary>
public class LevelGlobal : Singleton<LevelGlobal>
{
    private readonly Dictionary<Type, ALevelDataBase> lifeCache = new Dictionary<Type, ALevelDataBase>();
    
    /// <summary> 基本数据 </summary>
    private ALevelDataBase levelDB;
    
    /// <summary> 当前关卡 </summary>
    public ALevelDataBase DataBase => levelDB;

    /// <summary> 胜利状态 </summary>
    public bool IsRedWin { get; private set; }

    private ALevelDataBase GetLifeBase(Type t)
    {
        if (!lifeCache.TryGetValue(t, out var life) || life.IsNull())
        {
            life = ActivatorFactory.CreateInstance(t) as ALevelDataBase;
            lifeCache.Add(t, life);
        }
        return life;
    }
    
    public void Init<T>(int regionId, object args = null) where T : ALevelDataBase, new()
    {
        Type t = typeof(T);
        IsRedWin = false;
        levelDB = GetLifeBase(t);
        levelDB.Init(regionId, args);
    }

    public void SetLost(CampType campType)
    {
        IsRedWin = campType == CampType.蓝;
    }

    public void Clear()
    {
        levelDB?.Clear();
        levelDB = null;
    }
}
