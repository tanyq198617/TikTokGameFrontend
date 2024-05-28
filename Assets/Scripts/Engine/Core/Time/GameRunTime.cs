using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏运行时间
/// </summary>
public class GameRunTime
{
    /// <summary> 客户端帧计数 </summary>
    public static int GameFrameTime { set; get; }

    private static long m_SrvNowTime = 0;

    private static DateTime m_ClientNowTime;

    /// <summary> 对外接口：客户端当前时间 </summary>
    public static long ClientNow
    {
        get
        {
            return m_SrvNowTime + ClientTickTime;
        }
    }

    /// <summary> 客户端时间步进 </summary>
    private static long ClientTickTime
    {
        get { return (long)DateTime.Now.Subtract(m_ClientNowTime).TotalSeconds; }
    }

    /// <summary>
    /// 服务器时间设置入口
    /// </summary>
    public static void SetSrvTime(long srvTime)
    {
        m_ClientNowTime = DateTime.Now;
        m_SrvNowTime = srvTime / 1000;

        //var dataTime = TimeUtility.GetLongTime(ClientToSrvNowTime);
        //Debuger.LogError($"当前时间：{dataTime.ToString()}");
    }
}
