using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetConst 
{
    /// <summary> 默认资源包名 </summary>
    public const string DefaultPackage = "Main";

    /// <summary> 并发下载个数 </summary>
    public const int DownLoadingMaxNum = 50;

    /// <summary> 失败尝试次数 </summary>
    public const int FailedTryAgain = 3;

    /// <summary> 超时时间 </summary>
    public const int TimeOut = 120;

}
