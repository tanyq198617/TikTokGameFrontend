using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using GameNetwork;
using Sproto;
using YooAsset;

/// <summary>
/// 网络层管理器
/// </summary>
public class NetMgr : MonoBehaviour,IEvent
{
    private static NetMgr _instance;
    public static NetMgr Instance => _instance;
    
    
    /// <summary> 请求测试 </summary>
    public static event Action<bool> SayHelloCall;

    /// <summary> 连接回调 </summary>
    public static event Action ConnectCall;

    /// <summary> 断线 </summary>
    public static event Action DisconnectCall;

    /// <summary> 断线检测 </summary>
    public static event Func<bool> CheckConditionFunc;
    
    
    /// <summary> 网络层 </summary>
    private NetworkClient Network;
    private CancellationTokenSource source;
    
    public static T GetHandler<T>() where T : ATcpHandler => MsgRegister.GetHandle<T>();

    public bool IsConnected => Network != null && Network.IsConnected();

    private const float interval = 5;
    private float timer;
    
    private void Awake()
    {
        _instance = this;
        Network = new NetworkClient();
        source = new CancellationTokenSource();
        AddEventListener();
        DontDestroyOnLoad(this);
    }
    
    /// <summary>
    /// 初始化网络
    /// </summary>
    private void Start()
    {
        MsgRegister.OnRegister();
    }

    public void ConnectServer()
    {
        string ipStr;
        string portStr;
        
        if (Boot.Config.PlayMode == EPlayMode.EditorSimulateMode ||
            Boot.Config.IsLocalServerMode)
        {
             var arr = Boot.Config.GameServerIP.Split(GameKeyName.Record_Ten);
             ipStr = arr[0];
             portStr = arr[1];
        }
        else
        {
            ipStr = Boot.GetValue("ServerIP");
            portStr = Boot.GetValue("ServerPort");
        }

        if (string.IsNullOrEmpty(ipStr) || string.IsNullOrEmpty(portStr) ||
            !int.TryParse(portStr, out var port))
        {
            Debuger.LogFatal($"缺失IP地址或者端口号,IP={ipStr}, port={portStr}");
            return;
        }

        Network.SendConnect(ipStr, port);
    }
    
    public void SendClientKey()
    {
        Network.Clear();
        string clientKey = UEncrypt.GenerateRandom(8);
        UEncrypt.SetDesKey(clientKey);
        ByteBuffer buffer = new ByteBuffer();
        byte[] code = UEncrypt.RSAEncrypt(clientKey);
        buffer.WriteBytes(code);
        Network.SendMessageSync(buffer.ToBytes());
        Network.OnInit(UEncrypt.DESEncryptByBytes, UEncrypt.DESDecryptByBytes, DisconnectCall);
    }
    
    public void SendTo<T,K>(MsgRequestHandler<T> setReqHandler = null, MsgResponseHandler respHandler = null) 
        where T : SprotoTypeBase, new()
        => MsgWaiter.SendTo<T,K>(setReqHandler,respHandler);

    public void OnSocket(byte[] data)
    {
        Network?.SendMessageSync(data);
    }

    private void Update()
    {
        CheckHeartbeat();
    }
    
    private void CheckHeartbeat()
    {
        if (IsConnected && MathUtility.IsOutTime(ref timer, interval))
        {
            timer = 0;
            SendTo<SprotoType.heartbeat.request, C2SProtocol.heartbeat>();
        }
    }

    private void OnDestroy()
    {
        Network.OnRemove();
        MsgWaiter.Clear();
        RemoveEventListener();
    }


    public void AddEventListener()
    {
        EventMgr.AddEventListener(nameof(NetDebugChanged), NetDebugChanged);
    }

    public void RemoveEventListener()
    {
        EventMgr.RemoveEventListener(nameof(NetDebugChanged), NetDebugChanged);
    }
    
    private void NetDebugChanged()
    {
        NetDebug.OnShowLog(Boot.IsNetLog());
    }
}
