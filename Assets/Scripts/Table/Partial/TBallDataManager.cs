using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotUpdateScripts
{
    public partial class TBallDataManager
    {
        /// <summary> key=type, value=数据 </summary>
        private readonly Dictionary<int, TBallData> typeDict = new Dictionary<int, TBallData>();
        
        protected override void OnSet(int i, TBallData item)
        {
            base.OnSet(i, item);
            typeDict[item.type] = item;
        }

        /// <summary>
        /// 通过类型查找
        /// </summary>
        public TBallData GetItemByType(int type)
        {
            typeDict.TryGetValue(type, out var data);
            if (data == null)
                Debuger.LogError(typeof(TBallData).ToString() + "------[GetItemByType]未找到type----type = " + type);
            return data;
        }
    }
}