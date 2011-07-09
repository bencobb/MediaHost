using System.IO;

namespace MediaHost.Domain.Storage
{
    public interface IStorage
    {
        string StoreFile(Stream stream);
    }
}