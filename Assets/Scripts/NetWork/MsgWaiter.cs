using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Sproto;
using SprotoType;

namespace GameNetwork
{
    /// <summary> 客户端发给服务器消息时的回调 </summary>
    public delegate void MsgRequestHandler<T>(T req);
    
    /// <summary> 服务器返回客户端消息时的回调 </summary>
    public delegate void MsgResponseHandler(SprotoTypeBase resp);
    
    /// <summary>
    /// 网络消息等待器
    /// </summary>
    public class MsgWaiter
    {
        /// <summary> key=消息序号, value=请求类型 </summary>
        private static readonly Dictionary<long?, Type> reqMsgDict = new Dictionary<long?, Type>();
        
        /// <summary> 服务器返回客户端, key=消息序列, value=消息处理器 </summary>
        private static readonly Dictionary<long, ProtocolFunctionDictionary.typeFunc> sessionDict = new Dictionary<long, ProtocolFunctionDictionary.typeFunc>();

        /// <summary> 服务器返回客户端, key=消息序列, value=消息处理器 </summary>
        private static readonly Dictionary<long, MsgResponseHandler> respHandlerDict = new Dictionary<long, MsgResponseHandler>();

        private static readonly SprotoStream sendStream = new SprotoStream();
        private static readonly SprotoPack sendPack = new SprotoPack();
        private static readonly SprotoPack recvPack = new SprotoPack();

        private static readonly Queue<MsgResponseHandler> msgQueue = new Queue<MsgResponseHandler>();
        
        public const int MAX_PACK_LEN = (1 << 16) - 1;
        
        /// <summary> 消息唯一ID </summary>
        private static long? session = null;
        
        /// <summary> socket链接 </summary>
        private static Socket m_socket;
        
        public static void SendTo<T,K>(MsgRequestHandler<T> setReqHandler, MsgResponseHandler respHandler) where T : SprotoTypeBase, new() 
        {
            var req = new T();
            if (!(setReqHandler is null))
                setReqHandler(req);
            var pkg = new package();
            session ??= 0;
            session++;
            pkg.session = (long)session;

            int tag = C2SProtocol.Instance.Protocol[typeof(K)];
            reqMsgDict.Add(session, typeof(T));
            pkg.type = tag;
            
            //存数据
            sessionDict.Add((long)session, C2SProtocol.Instance.Protocol[tag].Response.Value);
            respHandlerDict.Add((long)session, respHandler);

            sendStream.Seek(0, SeekOrigin.Begin);
            int len = pkg.encode(sendStream);
            len += req.encode(sendStream);

            byte[] data = sendPack.pack(sendStream.Buffer, len);

            if (data.Length > MAX_PACK_LEN)
            {
                NetDebug.LogError($"消息发送失败! 包体过大，最大包体长度：{MAX_PACK_LEN}, 当前数据长度：{data.Length}");
                return;
            }
            sendStream.Seek(0, SeekOrigin.Begin);
            sendStream.WriteByte((byte)(data.Length >> 8));
            sendStream.WriteByte((byte)data.Length);
            sendStream.Write(data, 0, data.Length);
            
            try
            {
                NetDebug.Log($"[发送消息] [C2S] 发送请求, 消息ID={session} MSG={typeof(T)}");
                NetMgr.Instance.OnSocket(data);
            }
            catch (Exception e)
            {
                NetDebug.LogError($"客户端消息发送报错！MSG={req.ToString()} \nError={e.ToString()}");
            }
        }
       
        /// <summary>
        /// 解包
        /// </summary>
        public static void Decode(ByteBuffer msg)
        {
            byte[] msgdata = msg.ReadBytes(msg.buffLen);

            var pkg = new package();
            byte[] data = recvPack.unpack(msgdata);
            int offset = pkg.init(data);
            long session = (long)pkg.session;

            if (pkg.HasType)
            {
                var receive = S2CProtocol.Instance.Protocol.GenRequest((int)pkg.type, data, offset);
#if UNITY_EDITOR
                if (receive.GetType() != typeof(S2CProtocol.heartbeat))
                    NetDebug.Log($"[接收消息] [S2C] 收到注册消息，消息ID={pkg.type} MSG={receive.GetType()}");
#endif
                Loom.QueueOnMainThread(obj => EventMgr.Dispatch($"{SocketEvent.Socket_Receive}{pkg.type}", (SprotoTypeBase)obj), receive);
            }
            else
            {
                if (reqMsgDict.TryGetValue(session, out var respType))
                {
                    Loom.QueueOnMainThread(obj => NetDebug.Log($"[接收消息] [S2C] 收到返回消息，消息ID={pkg.session} MSG={respType}"), null);
                    reqMsgDict.Remove(session);
                }

                if (respHandlerDict.TryGetValue(session, out var respHandler) &&
                    sessionDict.TryGetValue(session, out var GenResponse))
                {
                    respHandlerDict.Remove(session);
                    sessionDict.Remove(session);
                    if (!(respHandler is null))
                    {
                        var func = GenResponse(data, offset);
                        Loom.QueueOnMainThread(f => { respHandler((SprotoTypeBase)f); }, func);
                    }
                }
            }
        }

        public static void Clear()
        {
            session = null;
            m_socket = null;
            reqMsgDict.Clear();
            sessionDict.Clear();
            respHandlerDict.Clear();
        }
    }
}
