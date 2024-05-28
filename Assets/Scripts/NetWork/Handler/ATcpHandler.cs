using System;
using Sproto;

namespace GameNetwork
{
    public abstract class ATcpHandler
    {
        public abstract void OnRegister();

        protected void Register<T>(int msgId, Action<T> handle) where T : SprotoTypeBase
            => EventMgr.AddEventListener($"{SocketEvent.Socket_Receive}{msgId}", handle);

        protected void UnRegister<T>(int msgId, Action<T> handle) where T : SprotoTypeBase
            => EventMgr.RemoveEventListener($"{SocketEvent.Socket_Receive}{msgId}", handle);
    }
}