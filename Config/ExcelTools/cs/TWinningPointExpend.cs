using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TWinningPointExpend
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 连胜点消耗 </summary>
    [NinoSerialize]
    public partial class TWinningPointExpend : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 礼物id </summary>
        [NinoMember(2)] public string giftId;

        /// <summary> 礼物个数 </summary>
        [NinoMember(3)] public int giftNum;

        /// <summary> 指令图片名字 </summary>
        [NinoMember(4)] public string instructImageName;

        /// <summary> 指令文本 </summary>
        [NinoMember(5)] public string instructText;

        /// <summary> 消耗数量 </summary>
        [NinoMember(6)] public int consumeCount;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 连胜点消耗管理器 </summary>
    
    public partial class TWinningPointExpendManager : TNinoManager <TWinningPointExpend, TWinningPointExpendManager>
    {
        public override string TableName()
        {
            return "d_winning.py.PointExpend";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
