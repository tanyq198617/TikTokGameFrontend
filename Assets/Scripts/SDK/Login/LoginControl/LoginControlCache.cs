using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginControlCache
{
    private static readonly Dictionary<LoginType, ALoginControl> cache = new Dictionary<LoginType, ALoginControl>();

    public static ALoginControl Get(LoginType type)
    {
        if (cache.TryGetValue(type, out var control))
        {
            return control;
        }
        control = Create(type);
        cache.Add(type, control);
        control.OnCreate();
        return control;
    }

    private static ALoginControl Create(LoginType type)
    {
        switch (type)
        {
            case LoginType.Tencent:
            case LoginType.极光登录:
                return new JiGuangLoginControl();
            case LoginType.普通:
            case LoginType.TapTap:
            default: return new AccountLoginControl();
        }
    }
}
