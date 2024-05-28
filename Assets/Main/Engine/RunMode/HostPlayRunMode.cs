using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

/// <summary>
/// 网络资源更新模型
/// </summary>
internal class HostPlayRunMode : ARunMode
{
    public override InitializeParameters GetInitializeParameters(ResourcePackage package, ServerVersion appVersion)
    {
        Debug.LogWarning($"[运行模式] 当前在线资源模式!");
        string defaultHostServer = RunMode.GetDefaultHostServer(appVersion.GetResVersion(package.PackageName));
        string fallbackHostServer = RunMode.GetDefaultHostServer(appVersion.GetResVersion(package.PackageName));
        var initParamenters = new HostPlayModeParameters();
        initParamenters.DecryptionServices = new AssetDecryption();
        initParamenters.BuildinQueryServices = new GameQueryServices();
        initParamenters.DeliveryQueryServices = new DefaultDeliveryQueryServices();
        initParamenters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
        return initParamenters;
    }
}

/// <summary>
/// 远端资源地址查询服务类
/// </summary>
internal class RemoteServices : IRemoteServices
{
    private readonly string _defaultHostServer;
    private readonly string _fallbackHostServer;

    public RemoteServices(string defaultHostServer, string fallbackHostServer)
    {
        _defaultHostServer = defaultHostServer;
        _fallbackHostServer = fallbackHostServer;
    }
    string IRemoteServices.GetRemoteFallbackURL(string fileName)
    {
        return $"{_defaultHostServer}/{fileName}";
    }
    string IRemoteServices.GetRemoteMainURL(string fileName)
    {
        return $"{_fallbackHostServer}/{fileName}";
    }
}