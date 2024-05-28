using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间数据
/// </summary>
public class RoomModel : Singleton<RoomModel>
{
    /// <summary> 房间ID </summary>
    public string RoomID { get; private set; }

    /// <summary>
    /// 收到服务器消息
    /// 获得房间ID
    /// </summary>
    public void S2C_RetRoomInfo(string roomid)
    {
        this.RoomID = roomid;
    }

    public void Clear()
    {
        this.RoomID = string.Empty;
    }
}
