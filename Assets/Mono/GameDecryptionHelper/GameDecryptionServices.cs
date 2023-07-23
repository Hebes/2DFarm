﻿using System;
using System.IO;
using YooAsset;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    资源解密类 https://www.yooasset.com/docs/guide-runtime/CodeTutorial1

-----------------------*/

public class GameDecryptionServices : IDecryptionServices
{
    public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
    {
        return 32;
    }

    public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
    {
        throw new NotImplementedException();
    }

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

public class BundleStream : FileStream
{
    public const byte KEY = 64;

    public BundleStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
    {
    }
    public BundleStream(string path, FileMode mode) : base(path, mode)
    {
    }

    public override int Read(byte[] array, int offset, int count)
    {
        var index = base.Read(array, offset, count);
        for (int i = 0; i < array.Length; i++)
        {
            array[i] ^= KEY;
        }
        return index;
    }
}