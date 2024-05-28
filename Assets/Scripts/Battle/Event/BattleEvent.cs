using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗消息
/// </summary>
public class BattleEvent
{
    /// <summary> 尝试进入战斗场景 </summary>
    public const string Battle_TryEnter_Scene = "BattleTryEnterScene";
    
    /// <summary> 游戏开始 </summary>
    public const string Battle_GameIsStart = "BattleGameIsStart";
    
    /// <summary> 游戏游戏开始 </summary>
    public const string Battle_Game_Begin = "BattleGameBegin";
    
    /// <summary> 游戏结束 </summary>
    public const string Battle_GameIsOver = "BattleGameIsOver";
    
    /// <summary> 重新开始 </summary>
    public const string Battle_RestartGame = "BattleRestartGame";
    
    /// <summary> 角色数值改变 </summary>
    public const string Battle_Player_ValueChanged = "BattlePlayerValueChanged";
    
    /// <summary> 收到服务器消息 --> 玩家评论 </summary>
    public const string Battle_S2C_Comment = "BattleS2CComment";
    
    /// <summary> 收到服务器消息 --> 用户刷礼物 </summary>
    public const string Battle_S2C_Gift = "BattleS2CGift";
    
    /// <summary> 收到服务器消息 --> 用户点赞 </summary>
    public const string Battle_S2C_Like = "BattleS2CLike";
    
    /// <summary> 收到服务器消息 --> 连胜内容 </summary>
    public const string Battle_S2C_WinComboContent = "BattleS2CWinComboContent";
    
    
    
    /// <summary> 收到服务器消息 --> 开始发送排行榜信息 </summary>
    public const string Battle_S2C_Rank_Start = "BattleS2CRankStart";
    
    /// <summary> 收到服务器消息 --> 结束发送排行榜信息 </summary>
    public const string Battle_S2C_Rank_End = "BattleS2CRankEnd";
        
    /// <summary> 球球数量发生变化 </summary>
    public const string Battle_Ball_ValueChanged = "BattleBallValueChanged";
    
    /// <summary> 球球地图选择 </summary>
    public const string Battle_Map_Selected = "BattleMapSelected";  
    
    /// <summary> BOSS血量变化 </summary>
    public const string Battle_BOSS_HP_Changed = "BattleBOSSHPChanged";  

        
    /// <summary> 上报数据成功 </summary>
    public const string Battle_S2C_RetKillRecord = "BattleS2CRetKillRecord";
    
    /// <summary> 上报结算结果成功 </summary>
    public const string Battle_S2C_RetGameResult = "BattleS2CRetGameResult";
    
    /// <summary> 红蓝方的总的连胜数量变化</summary>
    public const string Battle_WinningTreak_Changed = "Battle_WinningTreak_Changed";  
    
    /// <summary> 大哥发生变化 </summary>
    public const string Battle_RickMan_Changed = "BattleRickManChanged";  
    
    /// <summary> 屏幕震动 </summary>
    public const string Battle_Camera_Shake = "BattleCameraShake";  
    
    /// <summary> BUFF </summary>
    public const string Battle_BUFF_Changed = "BattleBUFFChanged";  
    
    /// <summary> BUFF </summary>
    public const string Battle_BUFF_Recycle = "BattleBUFFRecycle";  
    
    /// <summary> 相机旋转 </summary>
    public const string Battle_Camera_Rotation = "Battle_Camera_Rotation"; 
    
    /// <summary> 添加小弟数据 </summary>
    public const string Battle_Add_YoungerBrother = "BattleAddYoungerBrother"; 
    
    /// <summary> 添加小弟数据 </summary>
    public const string Battle_Changed_YoungerBrother = "BattleChangedYoungerBrother"; 
    
    /// <summary> 刷礼物增加护盾 </summary>
    public const string Battle_AddShield = "BattleAddShield"; 
      
    /// <summary> 播放点赞提示 </summary>
    public const string Battle_PlayLikeTip = "Battle_PlayLikeTip";
    
    /// <summary> 结算动画结束 </summary>
    public const string Battle_GameOverAnimatorEnd = "Battle_GameOverAnimatorEnd";
    /// <summary> 连胜点消耗提示 </summary>
    public const string Battle_WinningPoint_ExpendTip = "Battle_WinningPoint_ExpendTip";
}
