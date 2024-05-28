using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TAudioData
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 音效 </summary>
    [NinoSerialize]
    public partial class TAudioData : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 音效名 </summary>
        [NinoMember(2)] public string audioName;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 音效管理器 </summary>
    
    public partial class TAudioDataManager : TNinoManager <TAudioData, TAudioDataManager>
    {
        public override string TableName()
        {
            return "d_audio.py.data";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
