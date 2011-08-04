using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Amazon;
using Amazon.S3;

namespace MediaHost.Domain.Storage
{
    public class StorageS3 : IStorage
    {
        public string StoreFile(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
