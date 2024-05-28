using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotUpdateScripts
{
    public interface IJsonItem
    {
        int Key();
    }


    public interface IJsonManager
    {
        string TableName();

        string TablePath();

        object TableData { get; }
    }


    public abstract class TJsonManager<T, TU> : Singleton<TU>, IJsonManager where T : IJsonItem where TU : new()
    {
        public object TableData { get { return _mItemArray; } }

        protected Action<T> onParseData;

        readonly T[] _mItemArray;
        readonly Dictionary<string, string> _mDescDic = new Dictionary<string, string>();
        readonly Dictionary<int, int> _mKeyItemMap = new Dictionary<int, int>();

        internal TJsonManager()
        {
            _mItemArray = TJsonParser.Parse<T>(TableName(), TablePath(), ref _mDescDic);
            if (_mItemArray == null)
            {
                return;
            }

            Init();

            for (int i = 0; i < _mItemArray.Length; i++)
            {
                _mKeyItemMap[_mItemArray[i].Key()] = i;
                onParseData?.Invoke(_mItemArray[i]);
            }
        }

        public T[] ReloadTable()
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            return TJsonParser.Parse<T>(TableName(), TablePath(), ref array);
        }

        protected virtual void Init() { }

        public T GetItem(int key, bool isLogError = true)
        {
            int itemIndex;
            if (_mKeyItemMap.TryGetValue(key, out itemIndex))
            {
                if (_mItemArray[itemIndex] == null && isLogError)
                    Debuger.LogError(typeof(T).ToString() + "------未找到key----key = " + key);
                return _mItemArray[itemIndex];
            }
            else
            {
                if (isLogError)
                    Debuger.LogError(typeof(T).ToString() + "------未找到key----key = " + key);
            }
            return default(T);
        }

        public string GetDesc(string propertyName)
        {
            if (_mDescDic.ContainsKey(propertyName))
                return _mDescDic[propertyName];
            return default(string);
        }

        public T[] GetAllItem()
        {
            return _mItemArray;
        }



        public abstract string TableName();
        public abstract string TablePath();
    }
}