using System;
using System.Collections.Generic;
using System.Reflection;

public class ActivatorFactory
{
    private static readonly Dictionary<Type, ConstructorInfo> constructorCache = new Dictionary<Type, ConstructorInfo>();

    /// <summary>
    /// 创建一个新的实例
    /// </summary>
    /// <typeparam name="T">实例类型</typeparam>
    /// <returns></returns>
    public static T CreateInstance<T>()
    {
        Type type = typeof(T);
        ConstructorInfo constructor = null;
        if (constructorCache.ContainsKey(type))
        {
            constructor = constructorCache[type];
        }
        else
        {
            constructor = type.GetConstructor(Type.EmptyTypes);
            constructorCache[type] = constructor;
        }
        return (T)constructor.Invoke(null);
    }

    /// <summary>
    /// 创建一个新的实例
    /// </summary>
    /// <typeparam name="Type">实例类型</typeparam>
    /// <returns></returns>
    public static object CreateInstance(Type type)
    {
        ConstructorInfo constructor = null;
        if (constructorCache.ContainsKey(type))
        {
            constructor = constructorCache[type];
        }
        else
        {
            constructor = type.GetConstructor(Type.EmptyTypes);
            constructorCache[type] = constructor;
        }
        return constructor.Invoke(null);
    }

    /// <summary>
    /// 创建一个新的实例
    /// </summary>
    /// <typeparam name="typeName">类型名字</typeparam>
    /// <returns></returns>
    public static object CreateInstance(string typeName)
    {
        Type type = Type.GetType(typeName);
        if (type == null) return null;
        return CreateInstance(type);
    }
}