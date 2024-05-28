using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public static class AssemblyExtention
{
    /// <summary>
    /// 获得抽象类的所有子类
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetAllSubclass(this Type type)
    {
        return GetAllSubclass(type.Assembly, type);
    }

    public static IEnumerable<Type> GetAllSubclass(this Assembly assembly, Type type)
    {
        List<Type> subClasses = new List<Type>();
        Parallel.ForEach(assembly.GetExportedTypes(), subtype =>
        {
            if (subtype.IsClass && subtype.IsSubclassOf(type))
            {
                lock (subClasses)
                {
                    subClasses.Add(subtype);
                }
            }
        });
        return subClasses;
    }


    public static IEnumerable<Type> FindAllClass(this Assembly assembly, Func<Type, bool> getter)
    {
        List<Type> subClasses = new List<Type>();
        Parallel.ForEach(assembly.GetExportedTypes(), subtype =>
        {
            if (getter(subtype))
            {
                lock (subClasses)
                {
                    subClasses.Add(subtype);
                }
            }
        });
        return subClasses;
    }

    /// <summary>
    /// 获得接口的所有子类
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetInterfaceAllSubclass(this Type interfaceType)
    {
        if (interfaceType == null) return null;
        List<Type> subClasses = new List<Type>();
        var assembly = Assembly.GetAssembly(interfaceType);
        Parallel.ForEach(assembly.GetExportedTypes(), type =>
        {
            if (type.IsClass && interfaceType.IsAssignableFrom(type))
            {
                lock (subClasses)
                {
                    subClasses.Add(type);
                }
            }
        });
        return subClasses;
    }

}
