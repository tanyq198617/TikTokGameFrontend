using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameNetwork
{
    public class SocketEvent
    {
        /// <summary> 网络链接超时 </summary>
        public const string Socket_TimeOut = "SocketTimeOut";

        /// <summary> 尝试网络链接 </summary>
        public const string Socket_CreateConnect = "SocketCreateConnect";

        /// <summary> 网络链接成功 </summary>
        public const string Socket_Success = "SocketSuccess";

        /// <summary> 网络链接断开 </summary>
        public const string Socket_Disconnect = "SocketDisconnect";

        /// <summary> 网络链接被关闭 </summary>
        public const string Socket_Connection_Closed = "SocketConnectionClosed";

        /// <summary> 网络链接失败 </summary>
        public const string Socket_Failed = "SocketFailed";


        /// <summary> 收到消息 </summary>
        public const string Socket_Receive = "SocketReceive";

        public const int InnerMsgID = 1024; //Socket 连接
        public const int Connect = 101;     //Socket 连接
        public const int Disconnect = 102;  //Socket 断开
    }
}