using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameNetwork
{
    public delegate byte[] EncryptFun(byte[] data);
    
    public interface ISocketClient
    {
        void OnInit(EncryptFun encrypt, EncryptFun decrypt, Action socketBroken);
        void OnRemove();
        void SetRecvMaxSize(int maxSize);
        void SendConnect(string ip, int port);
        void Close();
        void SendMessageSync(byte[] bytes);
        bool IsConnected();
    }
}