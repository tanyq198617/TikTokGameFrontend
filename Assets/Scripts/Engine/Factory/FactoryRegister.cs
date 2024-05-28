using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class FactoryRegister
{
    private static bool IsInit = false;
    
    public static void Initialize()
    {
        // WatchTime.Start();
        IsInit = false;
        //初始化注册
        AutoRegister().Forget();
        // WatchTime.ShowTime($"预制体[Initialize]加载用时：");
    }

    private static async UniTask AutoRegister()
    {
        var tasks = new List<UniTask>();
        var target = typeof(AFactory);
        var assembly = target.Assembly;
        var types = assembly.GetAllSubclass(target);
        foreach (var type in types)
        {
            var instance = type.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
            var instanceValue = (AFactory)instance.GetValue(null);
            tasks.Add(instanceValue.InitAsync());
        }
        await UniTask.WhenAll(tasks);
        IsInit = true;
    }
}
