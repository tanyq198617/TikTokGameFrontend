using System.Collections;
using System.Collections.Generic;
using HotUpdateScripts;
using UnityEngine;

namespace HotUpdateScripts
{
    public partial class TMapDataManager
    {
        /// <summary> 地图列表 /// </summary>
        public readonly List<TMapData> tMapDataList = new List<TMapData>();
        protected override void OnSet(int i, TMapData item)
        {
            base.OnSet(i, item);

            tMapDataList.Add(item);
        }
    }
}