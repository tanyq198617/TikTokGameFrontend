using UnityEngine;

namespace HotUpdateScripts
{
    public partial class TRobotDataManager
    {
        public TRobotData GetRobotData()
        {
           int index = Random.Range(1, _mItemArray.Length);
           return _mItemArray[index];
        }
    }
}