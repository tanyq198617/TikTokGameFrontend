using System.Collections.Generic;

namespace EventDispatcher
{
    public static class StringExtension
    {
        /// <summary> key=字符串 value=id </summary>
        private static readonly Dictionary<string, int> EventTypeHashMap = new Dictionary<string, int>();

        /// <summary> key=id value=字符串 </summary>
        private static readonly Dictionary<int, string> EventHashToStringMap = new Dictionary<int, string>();

        /// <summary>
        /// 当前ID
        /// </summary>
        private static int _currentId = 0;

        /// <summary>
        /// 字符串转hashID
        /// </summary>
        /// <param name="val">字符串Value</param>
        /// <returns></returns>
        public static int StringToHash(this string val)
        {
            if (EventTypeHashMap.TryGetValue(val, out var hashId))
            {
                return hashId;
            }

            hashId = ++_currentId;
            EventTypeHashMap[val] = hashId;
            EventHashToStringMap[hashId] = val;
            return hashId;
        }

        /// <summary>
        /// hashID转字符串
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static string HashToString(this int hash)
        {
            return EventHashToStringMap.TryGetValue(hash, out var value) ? value : string.Empty;
        }
    }
}