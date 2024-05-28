using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TOperateData
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 送礼 </summary>
    [NinoSerialize]
    public partial class TOperateData : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 礼物名字 </summary>
        [NinoMember(2)] public string giftName;

        /// <summary> 价值 </summary>
        [NinoMember(3)] public int price;

        /// <summary> 积分 </summary>
        [NinoMember(4)] public int score;

        /// <summary> 召唤基础积分 </summary>
        [NinoMember(5)] public int playerScore;

        /// <summary> 护盾 </summary>
        [NinoMember(6)] public int shield;

        /// <summary> BUFF </summary>
        [NinoMember(7)] public int buff;

        /// <summary> 转换球类型 </summary>
        [NinoMember(8)] public int[][] spwans;

        /// <summary> 带头像个数 </summary>
        [NinoMember(9)] public int headnum;

        /// <summary> ui显示文本 </summary>
        [NinoMember(10)] public string uiXIanShi;

        /// <summary> 球球个数 </summary>
        [NinoMember(11)] public int uiXIanShiCount;

        /// <summary> 弹窗类型(1,边缘弹窗,2中心弹窗,3两种都有) </summary>
        [NinoMember(12)] public int tanChuangType;

        /// <summary> 送礼物大弹窗,标题图片名 </summary>
        [NinoMember(13)] public string tipImageName;

        /// <summary> 礼物图标 </summary>
        [NinoMember(14)] public string giftImageName;

        /// <summary> 入场timeline </summary>
        [NinoMember(15)] public int ruChangTimeLine;

        /// <summary> 入场timeline </summary>
        [NinoMember(16)] public int jinJieTimeLine;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 送礼管理器 </summary>
    
    public partial class TOperateDataManager : TNinoManager <TOperateData, TOperateDataManager>
    {
        public override string TableName()
        {
            return "d_operate.py.data";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
