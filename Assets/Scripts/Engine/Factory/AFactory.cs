using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;
using Object = UnityEngine.Object;

/// <summary>
/// 所有工厂类的基类
/// </summary>
public abstract class AFactory
{
    public static GameObject defualtRoot;

    protected bool m_IsDefaultRoot = false;
    protected string m_prefabPath;
    protected string m_rootName;
    protected bool m_AutoLoad = false;
    
    private GameObject root = null;
    private GameObject m_prefab = null;
    private AssetOperationHandle m_handle = null;

    protected readonly Dictionary<Type, FactoryBase> _poolDict = new Dictionary<Type, FactoryBase>();

    protected readonly Dictionary<Type, HashSet<APoolGameObjectBase>> useDict = new Dictionary<Type, HashSet<APoolGameObjectBase>>();

    protected AFactory()
    {
        if (defualtRoot == null)
        {
            defualtRoot = new GameObject("~root");
            Object.DontDestroyOnLoad(defualtRoot);
        }
    }
    
    public void Init()
    {
        if (m_prefab == null)
        {
            root = m_IsDefaultRoot ? defualtRoot : new GameObject($"~{m_rootName}");
            m_handle = YooAssets.LoadAssetSync<GameObject>(m_prefabPath);
            m_prefab = m_handle.GetAssetObject<GameObject>();
        }
    }

    public async UniTask InitAsync()
    {
        if (m_prefab == null)
        {
            bool isDefualtRoot = m_IsDefaultRoot || string.IsNullOrEmpty(m_rootName);
            if (!m_IsDefaultRoot && string.IsNullOrEmpty(m_rootName))
                Debuger.LogWarning($"{Path.GetFileNameWithoutExtension(m_prefabPath)} 没有设置默认节点，也没有设置挂点名字，将使用默认节点挂载!");
            root =  isDefualtRoot ? defualtRoot : new GameObject($"~{m_rootName}");
            m_handle = YooAssets.LoadAssetAsync<GameObject>(m_prefabPath);
            await m_handle.ToUniTask();
            m_prefab = m_handle.GetAssetObject<GameObject>();
        }
    }

    public T Spwan<T>() where T : APoolGameObjectBase, new()
    {
        var type = typeof(T);
        if (!_poolDict.TryGetValue(type, out var _pool))
        {
            _pool = Factory<T>.Instance.Init(root.transform, m_prefab);
            _poolDict.Add(type, _pool);
        }
        var result = _pool.GetOrCreate<T>();
        if(!useDict.TryGetValue(type, out _))
            useDict.Add(type, new HashSet<APoolGameObjectBase>());
        useDict[type].Add(result);
        return result;
    }

    public void Recycle<T>(T item) where T : APoolGameObjectBase, new()
    {
        var type = typeof(T);
        if (_poolDict.TryGetValue(type, out var _pool))
        {
            _pool.Recycle(item);
        }

        if (useDict.TryGetValue(type, out var hashSet))
        {
            hashSet.Remove(item);
        }
    }

    public void RecycleAll()
    {
        foreach (var kv in useDict)
        {
            var list = kv.Value.ToList();
            var type = kv.Key;
            if (_poolDict.TryGetValue(type, out var _pool))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    _pool.Recycle(list[i]);
                }
            }
        }
        useDict.Clear();
    }

}
