using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using YooAsset;

namespace HotUpdateScripts
{
    public interface INinoItem
    {
        int Key();
    }


    public interface INinoManager
    {
        string TableName();

        string TablePath();

        object TableData { get; }

        UniTaskVoid LoadAsync();

        void OnDispose();
    }

    public abstract class TNinoManager<T, TU> : Singleton<TU>, INinoManager where T : INinoItem where TU : new()
    {
        public object TableData { get { return _mItemArray; } }

        /// <summary> 如果此值设置为false必须要手动加载配置表 </summary>
        protected virtual bool IsAutoLoad => true;

        protected bool IsLoaded = false;

        protected T[] _mItemArray;

        protected Dictionary<int, int> _mKeyItemMap = new Dictionary<int, int>();

        public CancellationTokenSource source;

        protected AssetOperationHandle assetHandle;

        internal TNinoManager()
        {
            if (IsAutoLoad)
            {
                Load();
            }
        }

        public virtual void Load()
        {
            Init();
            _mItemArray = TNinoParser.Parse<T>(TableName(), TablePath(), LoadBytes, LoadItem);
            TableRegister.UnLoad(PathConst.GetNinoPath(TablePath(), TableName()));
            InitSucceed();
            IsLoaded = true;
        }

        public virtual byte[] LoadBytes(string name, string path)
        {
            string resPath = PathConst.GetNinoPath(path, name);
            var bytes = TableRegister.GetTextAsset(resPath);
            if (bytes != null && bytes.Length > 0) return bytes;
            assetHandle = YooAssets.LoadAssetSync<TextAsset>(resPath);
            TextAsset asset = assetHandle.GetAssetObject<TextAsset>();
            return asset.bytes;
        }

        public async UniTask<byte[]> LoadBytesAsync(string name, string path)
        {
            string resPath = PathConst.GetNinoPath(path, name);
            var bytes = TableRegister.GetTextAsset(resPath);
            if (bytes != null && bytes.Length > 0) return bytes;
            assetHandle = YooAssets.LoadAssetAsync<TextAsset>(resPath);
            await assetHandle.ToUniTask();
            return assetHandle.GetAssetObject<TextAsset>().bytes;
        }

        public void LoadItem(int i, T item)
        {
            if (IsLoaded)
                return;
            _mKeyItemMap[item.Key()] = i;
            OnSet(i, item);
        }

        public async UniTaskVoid LoadAsync()
        {
            if (IsAutoLoad || IsLoaded)
                return;

            try
            {
                source = new CancellationTokenSource();
                _mItemArray = await TNinoParser.ParseAsync<T>(TableName(), TablePath(), LoadBytesAsync, LoadItem, source); 
                TableRegister.UnLoad(PathConst.GetNinoPath(TablePath(), TableName()));
                if (source == null || source.IsCancellationRequested)
                    return;
                InitSucceed();
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                if (!(ex is TaskCanceledException))
                {
                    throw ex;
                }
            }
        }

        protected virtual void Init() { }
        protected virtual void InitSucceed() { }
        protected virtual void OnSet(int i, T item) { }

        public T GetItem(int key, bool isLogError = true)
        {
            int itemIndex;
            if (_mKeyItemMap == null || _mKeyItemMap.Count <= 0)
            {
                Debuger.LogError(typeof(T).ToString() + "------配置表未初始化----");
                return default(T);
            }

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

        public T[] GetAllItem()
        {
            if (_mKeyItemMap == null || _mKeyItemMap.Count <= 0)
            {
                Debuger.LogError(typeof(T).ToString() + "------配置表未初始化----");
            }
            return _mItemArray;
        }

        public int GetItemCount() => _mKeyItemMap.Count;

        public abstract string TableName();
        public abstract string TablePath();

        public void OnDispose()
        {
            if (source != null)
            {
                source.Cancel(false);
                source.Dispose();
                source = null;
            }
            assetHandle?.Release();
            assetHandle = null;
        }
    }
}