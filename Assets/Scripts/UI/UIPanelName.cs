using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Spine;
using UnityEngine;

/// <summary>
/// 其他UI注册界面
/// </summary>
public partial class UIPanelName
{
    #region 通用界面
    public const string WaitcircleView = "uiview_waitcircle";//菊花
    public const string LoadingView = "uiview_loading";//加载Loading
    public const string LoginMaskView = "uiview_login_mask";//加载Loading
    public const string AlterView = "uiview_alter";//通用弹出提示面板
    public const string PopView = "uiview_pop";//通用提示条
    #endregion

    #region 登录创角
    public const string LoginView = "uiview_login";
    public const string JiGuangLoginView = "uiview_login_jiguang";   //极光登录
    public const string JiGuangCodeLoginView = "uiview_login_jiguang_code";   //极光登录
    public const string PrivacyView = "uiview_privacy"; //隐私
    #endregion

    #region 主界面
    public const string MainBottomView = "uiview_main_bottom";
    #endregion

    #region 商店
    public const string ShopView = "";
    #endregion

    #region 角色
    public const string RoleView = "uiview_role";
    #endregion

    #region 战斗
    public const string FightView = "uiview_fight";
    public const string BattleView = "uiview_battle";
    public const string MapView = "uiview_map";
    #endregion
    
    
    #region 战斗
    public const string TestView = "uiview_test";
    #endregion

    #region 武学
    public const string SkillView = "";
    #endregion

    #region 门派
    public const string UnionView = "";
    #endregion

    #region 开始游戏界面

    public const string StartGameView = "uiview_start_game";//游戏开始界面
    public const string GameStartAnimatorView = "uiview_gameStart_animator";//游戏开始界面

    #endregion
    
    #region GM

    public const string GmView = "uiview_gm";//gm界面

    #endregion
    
    #region 排行

    public const string RankView = "uiview_rank";//排行榜

    #endregion

    #region 玩家加入阵营

    public const string GiftSideView = "uiview_gift_side";//玩家加入阵营和送礼物

    #endregion

    #region buff
    
    public const string BuffView = "uiview_buff";//红蓝方buff
    
    #endregion

    public const string SettingView = "uiview_setting";//设置界面
}