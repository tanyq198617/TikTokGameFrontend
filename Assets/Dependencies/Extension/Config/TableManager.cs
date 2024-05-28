using System.Collections.Generic;

namespace HotUpdateScripts
{
    public interface ITableItem
    {
        int Key();
    }


    public interface ITableManager
    {
        string TableName();

        object TableData { get; }
    }

    public abstract class TableManager<T, TU> : Singleton<TU>, ITableManager where T : ITableItem where TU : new()
    {


        public object TableData { get { return _mItemArray; } }

        readonly T[] _mItemArray;
        readonly Dictionary<string, string> _mDescDic = new Dictionary<string, string>();
        readonly Dictionary<int, int> _mKeyItemMap = new Dictionary<int, int>();

        internal TableManager()
        {
            _mItemArray = TableParser.Parse<T>(TableName(), ref _mDescDic);
            if (_mItemArray == null)
            {
                return;
            }

            for (int i = 0; i < _mItemArray.Length; i++)
                _mKeyItemMap[_mItemArray[i].Key()] = i;

            Init();
        }

        public T[] ReloadTable()
        {
            Dictionary<string, string> array = new Dictionary<string, string>();
            return TableParser.Parse<T>(TableName(), ref array);
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
    }

}