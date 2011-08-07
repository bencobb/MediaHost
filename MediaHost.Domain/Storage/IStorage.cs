using System.IO;
using System;

namespace MediaHost.Domain.Storage
{
    public interface IStorage
    {
        string StoreFile(Stream stream);
        string StoreStreamingFile(Stream stream, string contentType);
        string GetFileUrl(string key, bool isSSL);
    }
}