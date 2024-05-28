using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace GameNetwork
{
    public enum NetCode
    {
        未知错误 = 0,
        连接成功 = 1,
        连接失败 = 2,
        连接关闭 = 3,
        主动断开 = 4,
        消息读取失败 = 5,
    }
    
    /// <summary>
    /// 网络解析层
    /// </summary>
    public class NetworkClient : ISocketClient
    {
        private readonly object m_locker = new object();
        private Socket m_socket;
        private readonly MemoryStream memStream;
        private readonly BinaryReader reader;
        private IAsyncResult connectAsyncResult;

        private EncryptFun encryptFun;
        private EncryptFun decryptFun;
        private bool isInit = false;

        private static int MAX_READ = (1 << 16) - 1; //65535
        private byte[] byteBuffer = new byte[MAX_READ];
        
        private readonly byte[] packUnsignedShort = new byte[2];
        
        private event Action SocketIsNull;

        #region 初始化网络
        
        public NetworkClient()
        {
            memStream = new MemoryStream();
            reader = new BinaryReader(memStream);
        }

        /// <summary> 初始化 </summary>
        public void OnInit(EncryptFun encrypt, EncryptFun decrypt, Action socketBroken)
        {
            encryptFun = encrypt;
            decryptFun = decrypt;
            SocketIsNull = SocketIsNull;
            isInit = true;
        }
        
        public void Clear()
        {
            encryptFun = null;
            decryptFun = null;
            SocketIsNull = null;
            isInit = false;
        }
        
        public void SetRecvMaxSize(int maxSize)
        {
            MAX_READ = maxSize;
            byteBuffer = new byte[MAX_READ];
        }

        /// <summary> 是否链接上网络 </summary>
        public bool IsConnected() => isInit && m_socket != null && m_socket.Poll(1000, SelectMode.SelectWrite);

        #endregion

        #region Socket连接服务器

        public void SendConnect(string host, int port)
        {
            try
            {
                Close();
                NetDebug.LogWarning($"尝试连接网络，IP={host}:{port}");
                IPAddress[] address = Dns.GetHostAddresses(host);
                if (address.Length == 0)
                {
                    // Debug.LogError("host invalid");
                    // NetworkManager.AddEvent(Protocal.Exception, null);
                    NetDebug.LogError($"[Host] IP地址不合法, host={host}");
                    // OnConnected(NetCode.连接失败);
                    EventMgr.Dispatch(SocketEvent.Socket_Failed);
                    return;
                }
                
                //ipv6环境下,需要把ip转成ipv6格式
                m_socket = address[0].AddressFamily == AddressFamily.InterNetworkV6 ? new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp) : new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //超时时间
                m_socket.SendTimeout = 500;
               
                m_socket.BeginConnect(host, port, new AsyncCallback(OnSocketConnect), m_socket);
            }
            catch (Exception e)
            {
                Close();
                NetDebug.LogError($"[Host] 请求Socket连接出错了!!! \nError={e.ToString()}");
                // OnConnected(NetCode.连接失败);
                EventMgr.Dispatch(SocketEvent.Socket_Failed);
            }
        }

        private void OnSocketConnect(IAsyncResult asr)
        {
            try
            {
                var result = m_socket.BeginReceive(byteBuffer, 0, MAX_READ, SocketFlags.None, OnRead, this);
                // OnConnected(NetCode.连接成功);
                Loom.QueueOnMainThread(_ => EventMgr.Dispatch(SocketEvent.Socket_CreateConnect, m_socket.RemoteEndPoint.ToString()), null);
            }
            catch (Exception e)
            {
                NetDebug.LogError($"[严重错误] Socket连接错误!!! \nError:{e.ToString()}");
                // OnConnected(NetCode.连接失败);
                EventMgr.Dispatch(SocketEvent.Socket_Failed);
            }
        }
        

        #endregion

        #region 写出数据到服务器

        /// <summary> 写出数据 </summary>
        public void SendMessage(ByteBuffer buffer)
        {
            NetDebug.LogError("未实现[ByteBuffer]的发送处理器...");
        }

        /// <summary> 写出数据 </summary>
        public void SendMessageSync(byte[] bytes)
        {
            if (m_socket == null || !m_socket.Connected)
            {
                NetDebug.Fatal($"消息发送失败，网络连接被断开了!!!!");
                SocketIsNull?.Invoke();
                return;
            }

            //加密消息
            if (encryptFun != null)
                bytes = encryptFun(bytes);

            int length = bytes.Length;

            //当大于MAX_PACK_LEN(65535)时，拆分发送
            var maxLen = MsgWaiter.MAX_PACK_LEN - packUnsignedShort.Length;
            
            for (int i = 0; i < length; i += maxLen)
            {
                int remainingBytes = Math.Min(maxLen, length - i);

                ushort messageLen = (ushort)remainingBytes;

                if (BitConverter.IsLittleEndian)
                {
                    packUnsignedShort[0] = (byte)((messageLen & 0xff00) >> 8);
                    packUnsignedShort[1] = (byte)(messageLen & 0x00ff);
                }
                else
                {
                    packUnsignedShort[0] = (byte)(messageLen & 0x00ff);
                    packUnsignedShort[1] = (byte)((messageLen & 0xff00) >> 8);
                }

                byte[] msg = new byte[remainingBytes + 2];
                Buffer.BlockCopy(packUnsignedShort, 0, msg, 0, 2);
                Buffer.BlockCopy(bytes, i, msg, 2, remainingBytes);

                // Uncomment the following lines for debugging
                //for (int j = 0; j < msg.Length; j++)
                //{
                //    Debug.LogError("msg: [" + j.ToString() + "]" + msg[j].ToString());
                //}

                WriteMessage(msg);
            }
        }
        
        /// <summary>
        /// 写数据
        /// </summary>
        private void WriteMessage(byte[] data)
        {
            lock (m_locker)
            {
                try
                {
                    if (data == null)
                    {
                        NetDebug.LogError($"[C2S] 发送消息给服务器失败! 数据为空!!!");
                        return;
                    }

                    if (data.Length <= 0)
                    {
                        NetDebug.LogError($"[C2S] 发送消息给服务器失败! 数据长度: lenght <= 0");
                        return;
                    }

                    m_socket.Send(data, data.Length, SocketFlags.None);
                }
                catch (Exception e)
                {
                    // Debug.LogError($"[C2S] 发送消息给服务器报错! \nError={e.Message}");
                    OnDisconnected(NetCode.连接关闭, e);
                }
            }
        }

        #endregion

        #region 读取消息
        
        /// <summary>
        /// 读取消息
        /// </summary>
        void OnRead(IAsyncResult asr)
        {
            //Debug.LogError(asr);
            if (m_socket == null)
                return;
            try
            {
                var bytesRead = 0;

                lock (m_locker)
                {
                    bytesRead = m_socket.EndReceive(asr);

                    if (bytesRead < 1)
                    {
                        //包尺寸有问题，断线处理
                        //OnDisconnected(DisType.Disconnect, "bytesRead < 1");
                        NetDebug.LogError($"[Receive] 读取服务器数据长度不对! bytesRead < 1");
                        return;
                    }
                }

                OnReceive(byteBuffer, bytesRead); //分析数据包内容，抛给逻辑层
                //分析完，再次监听服务器发过来的新消息
                BeginReceive();
            }
            catch (Exception ex)
            {
                //PrintBytes();
                // Debug.LogWarning("OnRead ex = " + ex.ToString());
                // NetDebug.Fatal($"[网络连接将被断开] 读取消息出错：Error={ex.Message}");
                OnDisconnected(NetCode.消息读取失败, ex);
            }
        }

        private void BeginReceive()
        {
            if (m_socket == null)
                return;
            lock (m_locker)
            {
                if (m_socket != null)
                {
                    Array.Clear(byteBuffer, 0, byteBuffer.Length); //清空数组
                    m_socket.BeginReceive(byteBuffer, 0, MAX_READ, SocketFlags.None, OnRead, this);
                }
            }
        }

        /// <summary>
        /// 剩余的字节
        /// </summary>
        private long RemainingBytes() => memStream.Length - memStream.Position;
        
        private void OnReceive(byte[] bytes, int length)
        {
            memStream.Seek(0, SeekOrigin.End);
            memStream.Write(bytes, 0, length);
            //Reset to beginning
            memStream.Seek(0, SeekOrigin.Begin);
            while (RemainingBytes() > 2)
            {
                ushort messageLen = reader.ReadUInt16();//包长度
                if (BitConverter.IsLittleEndian)
                {
                    messageLen = (ushort)(((messageLen & 0xff00) >> 8) | ((messageLen & 0x00ff) << 8));
                }

                if (RemainingBytes() >= messageLen)
                {
                    OnReceivedMessage(reader.ReadBytes(messageLen));
                }
                else
                {
                    //Back up the position two bytes
                    memStream.Position = memStream.Position - 2;
                    break;
                }
            }
            //Create a new stream with any leftover bytes
            byte[] leftover = reader.ReadBytes((int)RemainingBytes());
            memStream.SetLength(0);     //Clear
            memStream.Write(leftover, 0, leftover.Length);
        }
        
        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="ms"></param>
        private void OnReceivedMessage(byte[] message)
        {
            if (decryptFun != null)
            {
                message = decryptFun(message);
            }
            var buffer = new ByteBuffer(message);
            // NetworkManager.AddEvent(Protocal.Message, buffer);
            MsgWaiter.Decode(buffer);
        }

        #endregion

        #region 关闭和移除网络

        /// <summary> 移除网络 </summary>
        public void OnRemove() => this.Close();

        /// <summary>
        /// 关闭网络链接
        /// </summary>
        public void Close()
        {
            if (m_socket == null)
                return;

            try
            {
                m_socket.Shutdown(SocketShutdown.Both);
                m_socket.Close();
            }
            catch (Exception e)
            {
                NetDebug.LogWarning($"[Close] 主动关闭网络链接失败。 Error: {e.ToString()}" );
            }
            
            m_socket = null;
        }
        
        /// <summary>
        /// 丢失链接
        /// </summary>
        private void OnDisconnected(NetCode code, Exception e)
        {
            Close();   //关掉客户端链接
            NetDebug.Fatal($"网络断开，Time={DateTime.Now:yyyy-MM-dd hh-mm-ss}  原因：{code.ToString()}");
            Debug.LogException(e);
            EventMgr.Dispatch(SocketEvent.Socket_Failed);
        }
        #endregion
    }
}