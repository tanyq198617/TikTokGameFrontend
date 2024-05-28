using System.Collections.Generic;
using System.Text;
using HotUpdateScripts;

/// <summary>
/// 首充管理器
/// </summary>
public class FirstGiftMgr : Singleton<FirstGiftMgr>
{
    /// <summary> 首充过的玩家openid </summary>
    protected readonly HashSet<string> firstlyPlayer = new HashSet<string>();
    
    /// <summary> 首充个数 </summary>
    protected int firstly;

    //日志
    private readonly StringBuilder builder = new StringBuilder();
    
    public void initialize()
    {
        firstly = TGlobalDataManager.Instance.GetByKey<int>(TGlobal.FirstlyGiftMaxPlayer);
        firstlyPlayer.Clear();
    }

    public void CheckFirstly(PlayerInfo info)
    {
        if (firstly <= 0)
            return;
        
        //如果充值过了，则不再执行
        if(!firstlyPlayer.Add(info.openid))
            return;

        builder.Clear();
        builder.Append($"[{info.nickname}({info.openid})] 触发首充赠送：");
        
        //执行首冲赠送
        for (int i = 0; i < TGlobalDataManager.FirstlyGiftExtra.Length; i++)
        {
            var key = TGlobalDataManager.FirstlyGiftExtra[i][0];
            var value = TGlobalDataManager.FirstlyGiftExtra[i][1];
            ShotFactory.OnShotBall(info, key, value);
            builder.Append($"赠送球球Type={key} 赠送球球数量={value} "); 
        }
        
        Debuger.Log(builder.ToString()); 
    }
}
