using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TFirePoint
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 发射点 </summary>
    [NinoSerialize]
    public partial class TFirePoint : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 预制体名字 </summary>
        [NinoMember(2)] public string prefabName;

        /// <summary> 所属发射器 </summary>
        [NinoMember(3)] public int mountType;

        /// <summary> 距离发射点距离 </summary>
        [NinoMember(4)] public double distance;

        /// <summary> 发射间隔 </summary>
        [NinoMember(5)] public double fireInterval;

        /// <summary> 加速间隔 </summary>
        [NinoMember(6)] public double acceleInterval;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 发射点管理器 </summary>
    
    public partial class TFirePointManager : TNinoManager <TFirePoint, TFirePointManager>
    {
        public override string TableName()
        {
            return "d_fire.py.Point";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
