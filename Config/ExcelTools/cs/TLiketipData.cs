using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TLiketipData
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 点赞提示 </summary>
    [NinoSerialize]
    public partial class TLiketipData : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 图片精灵名字 </summary>
        [NinoMember(2)] public string spriteName;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 点赞提示管理器 </summary>
    
    public partial class TLiketipDataManager : TNinoManager <TLiketipData, TLiketipDataManager>
    {
        public override string TableName()
        {
            return "d_liketip.py.data";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
