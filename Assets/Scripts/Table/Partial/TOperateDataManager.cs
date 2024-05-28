using System.Collections;
using System.Collections.Generic;
using Nino.Serialization;
using UnityEngine;

namespace HotUpdateScripts
{
    public partial class TOperateData
    {
        [NinoIgnore] public readonly Dictionary<int, int> spwanDict = new Dictionary<int, int>();
        [NinoIgnore] public readonly List<int> spwanKeys = new List<int>();

        public void OnInit()
        {
            for (int i = 0; i < spwans.Length; i++)
            {
                var arr = spwans[i];
                var key = arr[0];
                var value = arr[1];
                spwanDict[key] = value; 
                spwanKeys.Add(key);
            }
        }

        public int GetValue(int key)
        {
            spwanDict.TryGetValue(key, out var value);
            return value;
        }
    }

    public partial class TOperateDataManager
    {
        private readonly Dictionary<string, TOperateData> _operateDatas = new Dictionary<string, TOperateData>();
        
        protected override void OnSet(int i, TOperateData item)
        {
            base.OnSet(i, item);
            item.OnInit();
            _operateDatas[$"{item.Id}"] = item;
        }

        public TOperateData GetItem(string key)
        {
            _operateDatas.TryGetValue(key, out var value);
            return value;
        }
    }
}