using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotUpdateScripts
{
    public partial class TOperateExtraManager
    {

        /// <summary> key=礼物类型, value=赠送数量 </summary>
        public readonly Dictionary<string, List<TOperateExtra>> extraDict = new Dictionary<string, List<TOperateExtra>>();

        protected override void OnSet(int i, TOperateExtra item)
        {
            base.OnSet(i, item);

            if (!extraDict.TryGetValue(item.gift_id, out _))
                extraDict[item.gift_id] = new List<TOperateExtra>();
            extraDict[item.gift_id].Add(item);
        }

        public List<TOperateExtra> GetExtras(string gift_id)
        {
            extraDict.TryGetValue(gift_id, out var list);
            return list;
        }
    }
}