
using System;
using Cysharp.Threading.Tasks;

public class StartNetNode : AStateNode
{
    private bool isTryConnect = false;

    protected override void Begin()
    {
        //设置网络信息
        NetMgr.Instance.ConnectServer();
        OnWait().Forget();
        IsComplete = true;
    }

    public override void SysUpdate()
    {
        //初始化网络
        if (IsComplete && IsConnect())
        {
            _machine.RunNextNode(this);
        }
    }

    public async UniTask OnWait()
    { 
        var result = await UniTask.WhenAny
        (
            // UniTask.WaitUntil(()=> NetMgr.Instance.IsConnected),
            UniTask.Delay(TimeSpan.FromSeconds(3))
        );
        isTryConnect = true;
    }

    private bool IsConnect()
    {
        return NetMgr.Instance.IsConnected || isTryConnect;
    }
}
