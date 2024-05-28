using System;
using System.IO;
using YooAsset;

public class AssetDecryption : IDecryptionServices
{
    public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
    {
        return 32;
    }

    public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
    {
        throw new NotImplementedException();
    }

    //public FileStream LoadFromStream(DecryptFileInfo fileInfo)
    //{
    //    BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open);
    //    return bundleStream;
    //}

    public Stream LoadFromStream(DecryptFileInfo fileInfo)
    {
        BundleStream bundleStream = new BundleStream(fileInfo.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return bundleStream;
    }

    public uint GetManagedReadBufferSize()
    {
        return 1024;
    }

}
