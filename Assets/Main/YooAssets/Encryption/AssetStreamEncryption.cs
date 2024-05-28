using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

/// <summary>
/// 流加密
/// </summary>
public class AssetStreamEncryption : IEncryptionServices
{
    public EncryptResult Encrypt(EncryptFileInfo fileInfo)
    {
        // LoadFromStream
        if (fileInfo.BundleName.Contains("_gameres_audio"))
        {
            var fileData = File.ReadAllBytes(fileInfo.FilePath);
            for (int i = 0; i < fileData.Length; i++)
            {
                fileData[i] ^= BundleStream.KEY;
            }

            EncryptResult result = new EncryptResult();
            result.LoadMethod = EBundleLoadMethod.LoadFromStream;
            result.EncryptedData = fileData;
            return result;
        }

        // LoadFromFileOffset
        if (fileInfo.BundleName.Contains("_gameres_uiimage"))
        {
            var fileData = File.ReadAllBytes(fileInfo.FilePath);
            int offset = 32;
            var temper = new byte[fileData.Length + offset];
            Buffer.BlockCopy(fileData, 0, temper, offset, fileData.Length);

            EncryptResult result = new EncryptResult();
            result.LoadMethod = EBundleLoadMethod.LoadFromFileOffset;
            result.EncryptedData = temper;
            return result;
        }

        // Normal
        {
            EncryptResult result = new EncryptResult();
            result.LoadMethod = EBundleLoadMethod.Normal;
            return result;
        }
    }
}