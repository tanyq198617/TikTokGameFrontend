using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

internal class OfflineRunMode : ARunMode
{
    public override InitializeParameters GetInitializeParameters(ResourcePackage package, ServerVersion appVersion)
    {
        Debug.LogWarning($"[运行模式] 当前离线资源模式!");

        var initParamenters = new OfflinePlayModeParameters();
        initParamenters.DecryptionServices = new AssetDecryption();
        return initParamenters;
    }
}
