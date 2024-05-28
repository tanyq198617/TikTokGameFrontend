using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TGlobalData
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 全局表 </summary>
    [NinoSerialize]
    public partial class TGlobalData : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 键 </summary>
        [NinoMember(2)] public string Akey;

        /// <summary> 值 </summary>
        [NinoMember(3)] public string Val;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 全局表管理器 </summary>
    
    public partial class TGlobalDataManager : TNinoManager <TGlobalData, TGlobalDataManager>
    {
        public override string TableName()
        {
            return "d_global.py.data";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
