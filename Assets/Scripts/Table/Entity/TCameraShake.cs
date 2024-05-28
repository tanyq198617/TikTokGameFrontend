using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TCameraShake
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 相机振动 </summary>
    [NinoSerialize]
    public partial class TCameraShake : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 振动时间 </summary>
        [NinoMember(2)] public double time;

        /// <summary> 振动力度 </summary>
        [NinoMember(3)] public double shakeDynamics;

        /// <summary> 振动次数 </summary>
        [NinoMember(4)] public int shakeCount;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 相机振动管理器 </summary>
    
    public partial class TCameraShakeManager : TNinoManager <TCameraShake, TCameraShakeManager>
    {
        public override string TableName()
        {
            return "d_camera.py.shake";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
