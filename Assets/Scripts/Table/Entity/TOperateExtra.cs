using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TOperateExtra
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 额外赠送 </summary>
    [NinoSerialize]
    public partial class TOperateExtra : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 礼物类型 </summary>
        [NinoMember(2)] public string gift_id;

        /// <summary> 累计N个 </summary>
        [NinoMember(3)] public int compare;

        /// <summary> 赠送球球类型 </summary>
        [NinoMember(4)] public int ballid;

        /// <summary> 赠送个数 </summary>
        [NinoMember(5)] public int number;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 额外赠送管理器 </summary>
    
    public partial class TOperateExtraManager : TNinoManager <TOperateExtra, TOperateExtraManager>
    {
        public override string TableName()
        {
            return "d_operate.py.extra";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
