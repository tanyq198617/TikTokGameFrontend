using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

/// <summary>
/// 自动射击类
/// </summary>
public class AutoShotTick : ICompleted
{
    public string OpenID;
    public PlayerInfo Player;
    private TOperateData operateData;
    
    public bool isComplete;
    private float timer;
    private float interval;
    private int count;
    
    public void OnInit(PlayerInfo info, TOperateData data)
    {
        this.Player = info;
        this.OpenID = info.openid;
        this.operateData = data;
        this.interval = TGlobalDataManager.AutoShot_Interval;
        this.count = TGlobalDataManager.AutoShot_Count;
        this.isComplete = false;
    }

    public void Update()
    {
        if (MathUtility.IsOutTime(ref timer, interval))
        {
            timer = 0;
            //加入自动点赞子弹
            ShotFactory.OnPlayerShot(Player, operateData, count);
        }
    }

    public bool IsCompleted() => isComplete;

    public void Destory()
    {
        isComplete = true;
    }

    public void PutBackObj()
    {
        Clear();
        ClassFactory.Recycle(this);
    }

    private void Clear()
    {
        this.isComplete = true;
        this.Player = null;
        this.operateData = null;
        this.OpenID = string.Empty;
        this.interval = 0;
        this.count = 0;
    }
}
