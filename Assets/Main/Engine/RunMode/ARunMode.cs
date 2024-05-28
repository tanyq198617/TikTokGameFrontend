using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

/// <summary>
/// 项目运行模式
/// </summary>
internal abstract class ARunMode
{
    public abstract InitializeParameters GetInitializeParameters(ResourcePackage package, ServerVersion appVersion);
}
