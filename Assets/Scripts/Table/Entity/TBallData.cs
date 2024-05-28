using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TBallData
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 球 </summary>
    [NinoSerialize]
    public partial class TBallData : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 类型 </summary>
        [NinoMember(2)] public int type;

        /// <summary> 球球是属于大球还是小球概念 </summary>
        [NinoMember(3)] public int bigOrSmallBallTypr;

        /// <summary> 所属球球发射点 </summary>
        [NinoMember(4)] public int firePoint;

        /// <summary> 大小 </summary>
        [NinoMember(5)] public double size;

        /// <summary> 最小尺寸 </summary>
        [NinoMember(6)] public double minsize;

        /// <summary> 质量 </summary>
        [NinoMember(7)] public double mass;

        /// <summary> 生命 </summary>
        [NinoMember(8)] public int hp;

        /// <summary> 攻击力 </summary>
        [NinoMember(9)] public int attack;

        /// <summary> 结算间隔 </summary>
        [NinoMember(10)] public double resultInterval;

        /// <summary> 出生不碰撞时间 </summary>
        [NinoMember(11)] public double noColliderTime;

        /// <summary> 出生不碰撞范围 </summary>
        [NinoMember(12)] public double noColliderRange;

        /// <summary> 出膛速度 </summary>
        [NinoMember(13)] public double bornSpeed;

        /// <summary> 下落速度 </summary>
        [NinoMember(14)] public double moveSpeed;

        /// <summary> 允许的最大速度 </summary>
        [NinoMember(15)] public double maxSpeed;

        /// <summary> 效果范围 </summary>
        [NinoMember(16)] public double effectRange;

        /// <summary> 效果尺寸 </summary>
        [NinoMember(17)] public double effectScale;

        /// <summary> 效果持续时间 </summary>
        [NinoMember(18)] public double effectTime;

        /// <summary> 效果参数 </summary>
        [NinoMember(19)] public double effectParam;

        /// <summary> BUFF改变攻击 </summary>
        [NinoMember(20)] public int buffAttack;

        /// <summary> BUFF改变HP </summary>
        [NinoMember(21)] public int buffHP;

        /// <summary> 球球出场音效id </summary>
        [NinoMember(22)] public int beginSoundIndex;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 球管理器 </summary>
    
    public partial class TBallDataManager : TNinoManager <TBallData, TBallDataManager>
    {
        public override string TableName()
        {
            return "d_ball.py.data";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
