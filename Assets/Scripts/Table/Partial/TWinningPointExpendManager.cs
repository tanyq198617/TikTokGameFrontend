using System.Collections.Generic;

namespace HotUpdateScripts
{
    public partial class TWinningPointExpendManager
    {
        /// <summary> key=礼物类型, value=赠送数量 </summary>
        public readonly Dictionary<string, TWinningPointExpend> extraDict =
            new Dictionary<string, TWinningPointExpend>();

        protected override void OnSet(int i, TWinningPointExpend item)
        {
            base.OnSet(i, item);
            if (!extraDict.TryGetValue(item.instructText, out _))
                extraDict[item.instructText] = item;
        }

        /// <summary> 获取连胜点消耗数据陪表 /// </summary>
        public TWinningPointExpend GetWinningPointExpend(string instructText)
        {
            extraDict.TryGetValue(instructText, out var item);
            return item;
        }

        /// <summary> 判断是否含有连胜消耗配表 /// </summary>
        public bool CheckWinningPointExpend(string instructText)
        {
            extraDict.TryGetValue(instructText, out var item);
            return item != null;
        }
    }
}