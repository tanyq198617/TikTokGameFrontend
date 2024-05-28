using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TMapData
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 地图 </summary>
    [NinoSerialize]
    public partial class TMapData : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 场景名字 </summary>
        [NinoMember(2)] public string sceneName;

        /// <summary> 场景路径 </summary>
        [NinoMember(3)] public string scenePath;

        /// <summary> Boss血量 </summary>
        [NinoMember(4)] public int bossHp;

        /// <summary> 攻击力 </summary>
        [NinoMember(5)] public int attack;

        /// <summary> 受击扣血 </summary>
        [NinoMember(6)] public int[][] hurt;

        /// <summary> 受击间隔 </summary>
        [NinoMember(7)] public double interval;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 地图管理器 </summary>
    
    public partial class TMapDataManager : TNinoManager <TMapData, TMapDataManager>
    {
        public override string TableName()
        {
            return "d_map.py.data";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
