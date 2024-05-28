using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using UnityEngine.PlayerLoop;

public class CustomCertificateHandler : CertificateHandler
{
    // Encoded RSAPublicKey
    //private static readonly string PUB_KEY = "";

    /// <summary>
    /// Validate the Certificate Against the Amazon public Cert
    /// </summary>
    /// <param name="certificateData">Certifcate to validate</param>
    /// <returns></returns>
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}

/// <summary>
/// APP内下载冷更包
/// </summary>
public class AppDownLoadHelper
{
    public static async UniTask DownloadApp(string url, Action onStart, IProgress<float> onProgress, Action<bool, string> onState, CancellationTokenSource source)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.certificateHandler = new CustomCertificateHandler();
            onStart?.Invoke();
            try
            {
                await www.SendWebRequest().ToUniTask(onProgress, PlayerLoopTiming.Update, source.Token);
            }
            catch (Exception ex)
            {
                if (ex is TaskCanceledException)
                    return;
                else
                    throw ex;
            }

            if (www.IsError())
            {
                onState?.Invoke(false, $"版本异常：{www.error}");
                return;
            }

            string savePath = Path.Combine(Application.persistentDataPath, Path.GetFileName(url));
            if (!Directory.Exists(Application.persistentDataPath))
                Directory.CreateDirectory(Application.persistentDataPath);
            File.WriteAllBytes(savePath, www.downloadHandler.data);
            onState?.Invoke(true, savePath);
        }
    }

    private bool OnInstallApk(string apkPath)
    {
        bool result = false;
        try
        {
            result = InstallApp(apkPath);
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
    }


    /// <summary>
    /// 安装冷更包
    /// </summary>
    /// <param name="apkPath"></param>
    /// <returns></returns>
    public static bool InstallApp(string apkPath)
    {
        AndroidJavaClass javaClass = new AndroidJavaClass("com.example.install.Install");
        return javaClass.CallStatic<bool>("InstallApk", apkPath);
    }

    internal static Task DownloadApp(string androidApkUrl, Action onDownloadAppStart, object onDownloadAppProgress, Action<bool, string> onDownloadAppResult, CancellationTokenSource source)
    {
        throw new NotImplementedException();
    }
}
