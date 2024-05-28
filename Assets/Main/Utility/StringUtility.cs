using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class StringUtility 
{
    public const float Bytes2Mb = 1f / (1024 * 1024);

    public static string ToSizeStr(this long byteSize)
    {
        if (byteSize >= 1024 * 1024)
        {
            return $"{byteSize * Bytes2Mb:f2}MB";
        }
        if (byteSize >= 1024)
        {
            return $"{byteSize / 1024:f2}KB";
        }
        return $"{byteSize:f2}B";
    }
}
