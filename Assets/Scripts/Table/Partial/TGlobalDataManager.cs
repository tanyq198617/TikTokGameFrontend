using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HotUpdateScripts
{
    public class TGlobal
    {
        public const string KingEnterEffects = "KingEnterEffects"; //皇帝入场特效
        public const string CameraShake = "CameraShake"; //屏幕震动
        public const string FairyDragonBall = "FairyDragonBall"; //神龙炸弹数据
        public const string LongCameraShake = "LongCameraShake"; //屏幕震动
        public const string OutsideForce = "OutsideForce"; //超出边界后的力
        public const string smallExplosion = "smallExplosion"; //组合球（羊羊精灵+小爆破）,小爆破数据
        public const string smallExplosionGiftId = "smallExplosionGiftId"; //小爆破,礼物id
        public const string FairyDragonBallGiveGiftData = "FairyDragonBallGiveGiftData"; //玩家使用神龙炸弹,同时送的一些球球数据
        public const string HoleBallEffectSpeed = "HoleBallEffectSpeed"; //黑洞球特效生效速度
        public const string SmallExplosionTraliTime = "SmallExplosionTraliTime"; //小爆破拖尾时间
        public const string Thunderbolt = "Thunderbolt"; //组合球（恶魔炸弹+雷霆）
        public const string ThunderboltGiftId = "ThunderboltGiftId"; //雷霆,礼物id
        public const string RichManHeadEnlargement = "RichManHeadEnlargement"; //大哥头像放大,缩小
        public const string YoungerBrotherHeadEnlargement = "YoungerBrotherHeadEnlargement"; //小弟头像放大,缩小
        public const string AutoShot = "AutoShot"; //自动刷礼物
        public const string YoungerBrotherHeadCount = "YoungerBrotherHeadCount"; //小弟头像显示个数
        public const string PillBallSoundInterval = "PillBallSoundInterval"; //药丸球音效播放间隔
        public const string GrindBallSoundInterval = "GrindBallSoundInterval"; //碾压球音效播放间隔
        public const string LikeCount = "LikeCount"; //弹点赞神速的提示,点赞数量
        public const string KongTouTiShengScore = "KongTouTiShengScore"; //空投提升分数比例
        public const string WinningStreakTiShengScore = "WinningStreakTiShengScore"; //连胜提升分数比例
        public const string maxShield = "maxShield"; //护盾最大值
        public const string FirstlyGiftMaxPlayer = "FirstlyGiftMaxPlayer"; //允许首充玩家个数
        public const string FirstlyGiftExtra = "FirstlyGiftExtra"; //首充赠送
        public const string GiftIconTweenTime = "GiftIconTweenTime"; //玩家送礼,旋转缩小tween时间
        public const string GiftExplainTime = "GiftExplainTime";//礼物说明的切换时间
        public const string GiftExplainSpriteName = "GiftExplainSpriteName";//礼物说明的图片精灵名字
    }

    public partial class TGlobalDataManager
    {
        public readonly Dictionary<string, TGlobalData> globalDataDict = new Dictionary<string, TGlobalData>();
        public readonly Dictionary<string, object> objectDataDict = new Dictionary<string, object>();

        public readonly List<KingDataCfg> kingCfgList = new List<KingDataCfg>();
        public FairyDragonBallCfg fairyDragonBallCfg = new FairyDragonBallCfg();
        public SmallExplosionStruct smallExplosionStruct = new SmallExplosionStruct();

        public FairyDragonBallGiveGiftDataStruct _FairyDragonBallGiveGiftData = new FairyDragonBallGiveGiftDataStruct();
        public static float OutsideForce;
        public static int smallExplosionGiftId;
        public static float smallExplosionTraliTime;

        public SmallExplosionStruct thunderboltStruct = new SmallExplosionStruct();
        public static int thunderboltGiftId;

        public static PlayerHeadScaleTweenStruct richManHeadStruct;
        public static PlayerHeadScaleTweenStruct youngerBrotherHeadStruct;

        //自动刷礼物
        public static string AutoShot_CompareId;
        public static int AutoShot_GiftTotalNum;
        public static string AutoShot_GiftId;
        public static float AutoShot_Interval;
        public static int AutoShot_Count;

        //小弟头像显示个数
        public static int YoungerBrotherHeadCount;
        public static float PillBallSoundInterval;
        public static float GrindBallSoundInterval;

        public static int LikeCount;
        public static ScoreLiftStruct KongTouLiftStruct;
        public static ScoreLiftStruct WinningStreakLiftStruct;
        
        //最大护盾值
        public static int maxShield;
        
        //首充
        public static int[][] FirstlyGiftExtra;

        public static float GiftIconTweenTime;
        
        public static float GiftExplainTime;
        public static List<string> GiftExplainSpriteName;

        protected override void OnSet(int i, TGlobalData item)
        {
            base.OnSet(i, item);
            globalDataDict[item.Akey] = item;
            if (item.Akey.Equals(TGlobal.KingEnterEffects))
                SetKingCfgData(item.Val);
            else if (item.Akey.Equals(TGlobal.FairyDragonBall))
                SetFairyDragonBall(item.Val);
            else if (item.Akey.Equals(TGlobal.OutsideForce))
                float.TryParse(item.Val, out OutsideForce);
            else if (item.Akey.Equals(TGlobal.smallExplosion))
                SetSmallExplosionData(item.Val);
            else if (item.Akey.Equals(TGlobal.smallExplosionGiftId))
                int.TryParse(item.Val, out smallExplosionGiftId);
            else if (item.Akey.Equals(TGlobal.FairyDragonBallGiveGiftData))
                SetFairyDragonBallGiveGiftDataStruct(item.Val);
            else if (item.Akey.Equals(TGlobal.SmallExplosionTraliTime))
                float.TryParse(item.Val, out smallExplosionTraliTime);
            else if (item.Akey.Equals(TGlobal.Thunderbolt))
                SetThunderboltStruct(item.Val);
            else if (item.Akey.Equals(TGlobal.ThunderboltGiftId))
                int.TryParse(item.Val, out thunderboltGiftId);
            else if (item.Akey.Equals(TGlobal.RichManHeadEnlargement))
                SetRichManHeadStruct(item.Val);
            else if (item.Akey.Equals(TGlobal.YoungerBrotherHeadEnlargement))
                SetYoungerBrotherHeadStruct(item.Val);
            else if (item.Akey.Equals(TGlobal.AutoShot))
            {
                var autoShots = item.Val.Split(',');
                if (autoShots.Length < 5)
                    Debug.LogException(new Exception($"全局表字段配置错误，[{TGlobal.AutoShot}]长度至少5个字段!!"));
                AutoShot_CompareId = autoShots[0];
                int.TryParse(autoShots[1], out AutoShot_GiftTotalNum);
                AutoShot_GiftId = autoShots[2];
                float.TryParse(autoShots[3], out AutoShot_Interval);
                int.TryParse(autoShots[4], out AutoShot_Count);
            }
            else if (item.Akey.Equals(TGlobal.YoungerBrotherHeadCount))
                int.TryParse(item.Val, out YoungerBrotherHeadCount);
            else if (item.Akey.Equals(TGlobal.PillBallSoundInterval))
                float.TryParse(item.Val, out PillBallSoundInterval);
            else if (item.Akey.Equals(TGlobal.GrindBallSoundInterval))
                float.TryParse(item.Val, out GrindBallSoundInterval);
            else if (item.Akey.Equals(TGlobal.LikeCount))
                int.TryParse(item.Val, out LikeCount);
            else if (item.Akey.Equals(TGlobal.KongTouTiShengScore))
                SetKongTouLiftStruct(item.Val);
            else if (item.Akey.Equals(TGlobal.WinningStreakTiShengScore))
                SetWinningStreakLiftStruct(item.Val);
            else if (item.Akey.Equals(TGlobal.maxShield))
                int.TryParse(item.Val, out maxShield);
            else if (item.Akey.Equals(TGlobal.FirstlyGiftExtra))
                FirstlyGiftExtra = StringUtility.StringToNN(item.Val,GameKeyName.Slash, GameKeyName.Comma);
            else if (item.Akey.Equals(TGlobal.GiftIconTweenTime))
                float.TryParse(item.Val,out GiftIconTweenTime);
            else if (item.Akey.Equals(TGlobal.GiftExplainTime))
                float.TryParse(item.Val, out GiftExplainTime);
            else if (item.Akey.Equals(TGlobal.GiftExplainSpriteName))
                GiftExplainSpriteName = StringUtility.StringToListStr(item.Val, GameKeyName.Comma);
            
        }

        #region 设置数据

        /// <summary> 设置神龙炸弹数据 /// </summary>
        private void SetFairyDragonBall(string str)
        {
            var _strArry = StringUtility.StringToListStr(str, GameKeyName.Comma);

            fairyDragonBallCfg = new FairyDragonBallCfg();
            int.TryParse(_strArry[0], out fairyDragonBallCfg.giftId);
            int.TryParse(_strArry[1], out fairyDragonBallCfg.harm);
            int.TryParse(_strArry[2], out fairyDragonBallCfg.conversionRation);
            float.TryParse(_strArry[3], out fairyDragonBallCfg.attackInterval);
        }

        /// <summary> 生成大哥入场的结构体数据 /// </summary>
        private void SetKingCfgData(string str)
        {
            string[] strArry = str.Split('/');
            for (int i = 0, len = strArry.Length; i < len; ++i)
            {
                string[] _strArry = strArry[i].Split(',');
                KingDataCfg cfg = new KingDataCfg();
                int.TryParse(_strArry[0], out cfg.rankId);
                int.TryParse(_strArry[1], out cfg.ruchangPrefabType);
                cfg.audioName = _strArry[2];
                float.TryParse(_strArry[3], out cfg.audioyanShi);
                kingCfgList.Add(cfg);
            }
        }

        /// <summary> 生成雷霆一击的结构体数据 /// </summary>
        private void SetThunderboltStruct(string str)
        {
            var _strArry = StringUtility.StringToListStr(str, GameKeyName.Comma);

            thunderboltStruct = new SmallExplosionStruct();
            int.TryParse(_strArry[0], out thunderboltStruct.detectionRadius);
            int.TryParse(_strArry[1], out thunderboltStruct.totalTime);
            int.TryParse(_strArry[2], out thunderboltStruct.attackRadius);
            int.TryParse(_strArry[3], out thunderboltStruct.damage);
            int.TryParse(_strArry[4], out thunderboltStruct.resultDamage);
        }

        /// <summary> 玩家使用神龙炸弹,同时送的一些球球数据 /// </summary>
        private void SetFairyDragonBallGiveGiftDataStruct(string str)
        {
            var _strArry = StringUtility.StringToListStr(str, GameKeyName.Comma);

            _FairyDragonBallGiveGiftData = new FairyDragonBallGiveGiftDataStruct();
            int.TryParse(_strArry[0], out _FairyDragonBallGiveGiftData.giftId);
            long.TryParse(_strArry[1], out _FairyDragonBallGiveGiftData.giftNum);
        }

        private void SetSmallExplosionData(string str)
        {
            var _strArry = StringUtility.StringToListStr(str, GameKeyName.Comma);

            smallExplosionStruct = new SmallExplosionStruct();
            int.TryParse(_strArry[0], out smallExplosionStruct.detectionRadius);
            int.TryParse(_strArry[1], out smallExplosionStruct.totalTime);
            int.TryParse(_strArry[2], out smallExplosionStruct.attackRadius);
            int.TryParse(_strArry[3], out smallExplosionStruct.damage);
            int.TryParse(_strArry[4], out smallExplosionStruct.resultDamage);
        }

        /// <summary> 设置大哥的头像动画参数 /// </summary>
        private void SetRichManHeadStruct(string str)
        {
            var _strArry = StringUtility.StringToListStr(str, GameKeyName.Comma);

            richManHeadStruct = new PlayerHeadScaleTweenStruct();
            float.TryParse(_strArry[0], out var scale);
            richManHeadStruct.initHeadScale = new Vector3(scale, scale, scale);
            float.TryParse(_strArry[1], out richManHeadStruct.scaleTime);
        }

        /// <summary> 设置小弟的头像动画参数 /// </summary>
        private void SetYoungerBrotherHeadStruct(string str)
        {
            var _strArry = StringUtility.StringToListStr(str, GameKeyName.Comma);

            youngerBrotherHeadStruct = new PlayerHeadScaleTweenStruct();
            float.TryParse(_strArry[0], out var scale);
            youngerBrotherHeadStruct.initHeadScale = new Vector3(scale, scale, scale);
            float.TryParse(_strArry[1], out youngerBrotherHeadStruct.scaleTime);
        }

        /// <summary> 设置空投的提升比例数据 /// </summary>
        private void SetKongTouLiftStruct(string str)
        {
            var _strArry = StringUtility.StringToListStr(str, GameKeyName.Comma);

            KongTouLiftStruct = new ScoreLiftStruct();
            float.TryParse(_strArry[0], out  KongTouLiftStruct.scoerLift);
            float.TryParse(_strArry[1], out KongTouLiftStruct.maxScoerLift);
        }

        /// <summary> 设置连胜的提升比例数据 /// </summary>
        private void SetWinningStreakLiftStruct(string str)
        {
            var _strArry = StringUtility.StringToListStr(str, GameKeyName.Comma);

            WinningStreakLiftStruct = new ScoreLiftStruct();
            float.TryParse(_strArry[0], out  WinningStreakLiftStruct.scoerLift);
            float.TryParse(_strArry[1], out WinningStreakLiftStruct.maxScoerLift);
        }
        
        #endregion

        public string GetByKey(string _key)
        {
            return globalDataDict.TryGetValue(_key, out var item) ? item.Val : null;
        }

        public T GetByKey<T>(string _key) where T : struct, IConvertible
        {
            if (!objectDataDict.TryGetValue(_key, out var obj))
            {
                var str = GetByKey(_key);
                if (str.IsNullOrEmpty())
                {
                    return default;
                }

                obj = Convert.ChangeType(str, typeof(T));
                objectDataDict[_key] = obj;
            }

            return (T)obj;
        }
    }

    public struct KingDataCfg
    {
        public int rankId;
        public int ruchangPrefabType;
        public string audioName;
        public float audioyanShi;
    }

    /// <summary> 神龙炸弹的数据结构 /// </summary>
    public struct FairyDragonBallCfg
    {
        public int giftId;
        /// <summary> 伤害数量 /// </summary>
        public int harm;
        //炸弹兑换比例,比如10:1
        public int conversionRation; 
        //攻击间隔
        public float attackInterval;
    }

    /// <summary> 小爆破的数据结构 /// </summary>
    public struct SmallExplosionStruct
    {
        //检测目标半径
        public int detectionRadius;

        /// <summary> 播放时间随机总时长/// </summary>
        public int totalTime;

        //攻击范围半径
        public int attackRadius;

        //攻击力
        public int damage;

        //对基地的攻击力
        public int resultDamage;
    }

    /// <summary> 玩家使用神龙炸弹,同时送的一些球球数据 /// </summary>
    public struct FairyDragonBallGiveGiftDataStruct
    {
        //礼物id
        public int giftId;

        /// <summary> 礼物个数/// </summary>
        public long giftNum;
    }

    //大哥,小弟的头像放大缩小动画参数
    public struct PlayerHeadScaleTweenStruct
    {
        //头像初始大小
        public Vector3 initHeadScale;

        //动画时间
        public float scaleTime;
    }
    
    
    //结算分数提升数据结构
    public struct ScoreLiftStruct
    {
        //单个提升比例
        public float scoerLift;

        //最大提高数据
        public float maxScoerLift;
    }
}