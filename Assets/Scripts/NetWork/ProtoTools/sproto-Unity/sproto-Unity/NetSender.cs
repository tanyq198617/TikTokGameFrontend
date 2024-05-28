using System.Collections.Generic;
using Sproto;
using Debug = UnityEngine.Debug;

public delegate void RpcRspHandler(SprotoTypeBase rpcRsp);

public class NetSender
{
   // private static long session;
   // private static Dictionary<long, RpcRspHandler> rpcRspHandlerDict;

   // public static void Init()
   // {
   //     rpcRspHandlerDict = new Dictionary<long, RpcRspHandler>();
   // }

   // public static void Send<T>(SprotoTypeBase rpcReq = null, RpcRspHandler rpcRspHandler = null)
   // {
   //     if (rpcRspHandler != null)
   //     {
   //         session++;
   //         AddHandler(session, rpcRspHandler);
			//Debug.LogError(rpcReq);
   //         NetCore.Send<T>(rpcReq, session);
   //     }
   //     else
   //     {
			//Debug.LogError(rpcReq);
			//NetCore.Send<T>(rpcReq);
   //     }
   // }

   // private static void AddHandler(long session, RpcRspHandler rpcRspHandler)
   // {
   //     rpcRspHandlerDict.Add(session, rpcRspHandler);
   // }

   // private static void RemoveHandler(long session)
   // {
   //     if (rpcRspHandlerDict.ContainsKey(session))
   //     {
   //         rpcRspHandlerDict.Remove(session);
   //     }
   // }

   // public static RpcRspHandler GetHandler(long session)
   // {
   //     RpcRspHandler rpcRspHandler;
   //     rpcRspHandlerDict.TryGetValue(session, out rpcRspHandler);
   //     RemoveHandler(session);
   //     return rpcRspHandler;
   // }

}
