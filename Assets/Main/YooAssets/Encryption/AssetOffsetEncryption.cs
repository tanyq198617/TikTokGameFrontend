using System;
using System.IO;
using YooAsset;

/// <summary>
/// 偏移加密
/// </summary>
public class AssetOffsetEncryption : IEncryptionServices
{
    public EncryptResult Encrypt(EncryptFileInfo fileInfo)
    {
        //指定目录加密
        if (fileInfo.BundleName.Contains("_gameres_audio"))
        {
            int offset = 32;
            byte[] fileData = File.ReadAllBytes(fileInfo.FilePath);
            var encryptedData = new byte[fileData.Length + offset];
            Buffer.BlockCopy(fileData, 0, encryptedData, offset, fileData.Length);

            EncryptResult result = new EncryptResult();
            result.LoadMethod = EBundleLoadMethod.LoadFromFileOffset;
            result.EncryptedData = encryptedData;
            return result;
        }
        else
        {
            EncryptResult result = new EncryptResult();
            result.LoadMethod = EBundleLoadMethod.Normal;
            return result;
        }
    }
}


