using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TOperateEffect
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 影响发射器 </summary>
    [NinoSerialize]
    public partial class TOperateEffect : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 礼物类型 </summary>
        [NinoMember(2)] public string gift_id;

        /// <summary> 影响第几号发射器 </summary>
        [NinoMember(3)] public int fireIndex;

        /// <summary> 加速摆动时间 </summary>
        [NinoMember(4)] public double acceleTime;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 影响发射器管理器 </summary>
    
    public partial class TOperateEffectManager : TNinoManager <TOperateEffect, TOperateEffectManager>
    {
        public override string TableName()
        {
            return "d_operate.py.effect";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
