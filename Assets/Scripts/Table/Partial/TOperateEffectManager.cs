using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotUpdateScripts
{
    public partial class TOperateEffectManager
    {
        /// <summary> key=礼物类型, value=赠送数量 </summary>
        public readonly Dictionary<string, List<TOperateEffect>> extraDict = new Dictionary<string, List<TOperateEffect>>();

        protected override void OnSet(int i, TOperateEffect item)
        {
            base.OnSet(i, item);

            if (!extraDict.TryGetValue(item.gift_id, out _))
                extraDict[item.gift_id] = new List<TOperateEffect>();
            extraDict[item.gift_id].Add(item);
        }

        public List<TOperateEffect> GetEffects(string gift_id)
        {
            extraDict.TryGetValue(gift_id, out var list);
            return list;
        }
    }
}