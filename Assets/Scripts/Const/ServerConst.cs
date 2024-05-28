using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerConst
{
    /// <summary> 请求服务器,获得Ip和端口 </summary>
    public const string Hello_Server = "http://{0}:{1}/{2}?{3}";

    /// <summary> 中心服请求指令 </summary>
    public const string Hello_Cmd = "cmd=Hello";

    /// <summary> 获取中心服地址 </summary>
    public static string GetHelloUrl(string ip, int port, string instruction) => string.Format(Hello_Server, ip, port, instruction, Hello_Cmd);

    /// <summary> 获取中心服地址 </summary>
    public static string GetHelloUrl(string url, string instruction) => $"{url}{instruction}?{Hello_Cmd}";
}