using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TFireData
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 发射器 </summary>
    [NinoSerialize]
    public partial class TFireData : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 预制体名字 </summary>
        [NinoMember(2)] public string name;

        /// <summary> 摆动速度 </summary>
        [NinoMember(3)] public double speed;

        /// <summary> 摆动幅度 </summary>
        [NinoMember(4)] public double angle;

        /// <summary> 加速摆动 </summary>
        [NinoMember(5)] public double acceleSpeed;

        /// <summary> 加速摆动最大时间上限 </summary>
        [NinoMember(6)] public double acceleMaxTime;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 发射器管理器 </summary>
    
    public partial class TFireDataManager : TNinoManager <TFireData, TFireDataManager>
    {
        public override string TableName()
        {
            return "d_fire.py.data";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
