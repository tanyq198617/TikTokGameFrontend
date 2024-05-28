using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TBuffData
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> BUFF </summary>
    [NinoSerialize]
    public partial class TBuffData : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 结算间隔 </summary>
        [NinoMember(2)] public double time;

        /// <summary> 最多叠加层 </summary>
        [NinoMember(3)] public int max;

        /// <summary> 影响攻击力 </summary>
        [NinoMember(4)] public int attack;

        /// <summary> 影响生命值 </summary>
        [NinoMember(5)] public int hp;

        /// <summary> 影响皮肤 </summary>
        [NinoMember(6)] public int skin;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> BUFF管理器 </summary>
    
    public partial class TBuffDataManager : TNinoManager <TBuffData, TBuffDataManager>
    {
        public override string TableName()
        {
            return "d_buff.py.data";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
