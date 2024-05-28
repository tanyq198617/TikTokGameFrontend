using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 全局关卡数据
/// </summary>
public abstract class ALevelDataBase
{
    public TMapData MapSence { get; protected set; }
    
    public int RegionId { get; protected set; }
    public int MapId { get; protected set; }

    #region 初始化
    
    public void Init(int regionId, object args)
    {
        this.RegionId = regionId;
        MapSence = TMapDataManager.Instance.GetItem(regionId);
    }

    protected abstract void Release();
    
    #endregion


    #region API
    public string GetMapName() => MapSence?.scenePath ?? string.Empty;
    public int GetMaxHP() =>  MapSence?.bossHp ?? 0;
    public int GetAttack() =>  MapSence?.attack ?? 0;
    #endregion

    public void Clear()
    {
        MapSence = null;
        RegionId = 0;
        MapId = 0;
        Release();
    }

}
