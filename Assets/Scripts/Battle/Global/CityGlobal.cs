using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGlobal : Singleton<CityGlobal>
{
    /// <summary> 全局游戏结束 </summary>
    public bool IsOver;
    
    public void Release()
    {
        IsOver = true;
        BallFactory.Stop();
        ShotFactory.Stop();
        BallFactory.RecycleAll();
        BallEffectFactory.RecycleAll();
        FairyDragonBallFactory.RecycleAll();
        SmallExplosionController.Instance.Recycle();
        PlayerModel.Instance.Clear();
        RoomModel.Instance.Clear();
        RankModel.Instance.Clear();
        MapMgr.Instance.Close();
        BuffMgr.Release();
        TextureMgr.Instance.Release();
        CameraMgr.Instance.Clear();
        GiftExtraMgr.Clear();
    }
}
