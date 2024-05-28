using System.Collections.Generic;
using System.Collections;
using Nino.Serialization;

/**
 * TRobotData
 * 自动生成，请务修改
 */
namespace HotUpdateScripts
{
    /// <summary> 机器人 </summary>
    [NinoSerialize]
    public partial class TRobotData : INinoItem
    {
        /// <summary> 编号 </summary>
        [NinoMember(1)] public int Id;

        /// <summary> 名字 </summary>
        [NinoMember(2)] public string nickname;

        /// <summary> 头像 </summary>
        [NinoMember(3)] public string avatar_url;


        public virtual int Key()
        {
            return Id;
        }
}

    /// <summary> 机器人管理器 </summary>
    
    public partial class TRobotDataManager : TNinoManager <TRobotData, TRobotDataManager>
    {
        public override string TableName()
        {
            return "d_robot.py.data";
        }

        public override string TablePath()
        {
            return "table";
        }
    }
}
