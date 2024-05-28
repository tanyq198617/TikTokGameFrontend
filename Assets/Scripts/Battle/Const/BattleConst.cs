using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;

public class BattleConst
{
    public const string 发射器_通用 = "发射器_通用";
    
    public const string 发射点_简单小球 = "发射点_简单小球"; 
    public const string 发射点_简单大球 = "发射点_简单大球";
    public const string 发射点_爆炸球球 = "发射点_爆炸球球";
    public const string 发射点_药丸球球 = "发射点_药丸球球";
    public const string 发射点_黑洞球球 = "发射点_黑洞球球";
    public const string 发射点_碾压球球 = "发射点_碾压球球";
    
    public const string 发射点_特效_红 = "发射点_特效_红";
    public const string 发射点_特效_蓝 = "发射点_特效_蓝";
    
    public const string 球球_简单小球_红 = "球球_简单小球_红";
    public const string 球球_爆炸球球_红 = "球球_爆炸球球_红";
    public const string 球球_简单大球_红 = "球球_简单大球_红";
    public const string 球球_药丸球球_红 = "球球_药丸球球_红";
    public const string 球球_黑洞球球_红 = "球球_黑洞球球_红";
    public const string 球球_碾压球球_红 = "球球_碾压球球_红";
    public const string 球球_蛋仔球_红 = "球球_蛋仔球_红";
    public const string 球球_小爆炸球_红 = "球球_小爆炸球_红";
    
    public const string 球球_简单小球_蓝 = "球球_简单小球_蓝";
    public const string 球球_爆炸球球_蓝 = "球球_爆炸球球_蓝";
    public const string 球球_简单大球_蓝 = "球球_简单大球_蓝";
    public const string 球球_药丸球球_蓝 = "球球_药丸球球_蓝";
    public const string 球球_黑洞球球_蓝 = "球球_黑洞球球_蓝";
    public const string 球球_碾压球球_蓝 = "球球_碾压球球_蓝";
    public const string 球球_蛋仔球_蓝 = "球球_蛋仔球_蓝";
    public const string 球球_小爆炸球_蓝 = "球球_小爆炸球_蓝";

    /// <summary> 点赞礼物ID </summary>
    public const string gift_likeid = "4";

    /// <summary> 红方大哥的模型 /// </summary>
    public const string RedRichManModel = "dage_R";

    /// <summary> 蓝方大哥的模型 /// </summary>
    public const string BlueRichManModel = "dage_B";
    /// <summary> 大哥名字模型 /// </summary>
    public const string RichManNameModel = "richManName";

    #region 子弹碰撞或者死亡时的特效

    public const string 高级碰撞_红 = "E3D_pengzhuang_GJ_O";
    public const string 低级碰撞_红 = "E3D_pengzhuang_DJ_O";
    public const string 甜甜圈爆_红 = "E3D_tiantianquan_boom_O";
    public const string 黑洞_红 = "E3D_nengliangdianchi_heidong_O";
    public const string 进阶_红 = "E3D_jinjie_R";

    public const string 高级碰撞_蓝 = "E3D_pengzhuang_GJ_P";
    public const string 低级碰撞_蓝 = "E3D_pengzhuang_DJ_P";
    public const string 甜甜圈爆_蓝 = "E3D_tiantianquan_boom_P";
    public const string 黑洞_蓝 = "E3D_nengliangdianchi_heidong_P";
    public const string 进阶_蓝 = "E3D_jinjie_B"; 

    #endregion
    
    /// <summary> 蓝方神龙炸弹出场timeling /// </summary>
    public const string BlueFairyDragonChuChangTimeLine = "E3D_long_boom_B_chuchang";
  
    /// <summary> 红方神龙炸弹出场timeling  /// </summary>
    public const string RedFairyDragonChuChangTimeLine = "E3D_long_boom_R_chuchang";
  
    
    
    #region 组合球

    public const string 组合球_小爆破_红 = "组合球_小爆破_红";
    public const string 组合球_小爆破拖尾_红 = "E3D_xiaobaozha_trail_R";
    public const string 组合球_雷霆_红 = "E3D_nianyaqiu_elc_R";

    public const string 组合球_小爆破_蓝 = "组合球_小爆破_蓝";
    public const string 组合球_小爆破拖尾_蓝 = "E3D_xiaobaozha_trail_B";
    public const string 组合球_雷霆_蓝 = "E3D_nianyaqiu_elc_B";
    #endregion

    #region Timeline特效

    /// <summary> 蓝方升级TimeLing /// </summary>
    public const string 升级TimeLine_蓝 = "E3D_shengji_B";
    public const string 碾压球进阶_蓝_TimeLine = "E3D_NianYaQiu_jinjie_B_ChuChang_TimeLine";
    public const string 碾压球入场_蓝_TimeLine = "E3D_NianYaQiu_B_ChuChang_TimeLine";
    
  
    /// <summary> 红方升级蓝方升级_Timeline  /// </summary>
    public const string 升级TimeLine_红 = "E3D_shengji_R";
    public const string 碾压球进阶_红_TimeLine = "E3D_NianYaQiu_jinjie_R_ChuChang_TimeLine";
    public const string 碾压球入场_红_TimeLine = "E3D_NianYaQiu_R_ChuChang_TimeLine";

    #endregion
    
    #region timeline特效不带相机
    public const string 大哥退场_蓝_TimeLine = "E3D_dage_boom_B";
    public const string 大哥退场_红_TimeLine = "E3D_dage_boom_R";
    
    #endregion
}

public enum CampType
{
    NUll = 0,
    蓝 = 1,
    红 = 2,
}