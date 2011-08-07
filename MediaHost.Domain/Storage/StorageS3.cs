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
            string key = Guid.NewGuid().ToString("N");

            Amazon.S3.Model.PutObjectRequest por = new Amazon.S3.Model.PutObjectRequest();
            por.WithBucketName(Helper.AppConfig.AWS_Bucket)
                .WithKey(key)
                .WithInputStream(stream);

            Amazon.S3.Model.PutObjectResponse response = S3Helper.S3ClientInstance.PutObject(por);

            return key;
        }

        public string StoreStreamingFile(Stream stream, string contentType)
        {
            string key = Guid.NewGuid().ToString("N") + "." + contentType.Split('/').LastOrDefault();

            Amazon.S3.Model.PutObjectRequest por = new Amazon.S3.Model.PutObjectRequest();
            por.WithBucketName(Helper.AppConfig.AWS_BucketStreaming)
                .WithKey(key)
                .WithCannedACL(Amazon.S3.Model.S3CannedACL.PublicRead)
                .WithInputStream(stream);

            Amazon.S3.Model.PutObjectResponse response = S3Helper.S3ClientInstance.PutObject(por);

            return key;
        }

        public string GetFileUrl(string key, bool isSSL)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(30);

            Amazon.S3.Model.GetPreSignedUrlRequest req = new Amazon.S3.Model.GetPreSignedUrlRequest();
            req.WithBucketName(Helper.AppConfig.AWS_Bucket)
                .WithKey(key)
                .WithExpires(expiration)
                .WithVerb(Amazon.S3.Model.HttpVerb.GET);

            if (isSSL)
            {
                req.Protocol = Amazon.S3.Model.Protocol.HTTPS;
            }
            else
            {
                req.Protocol = Amazon.S3.Model.Protocol.HTTP;
            }

            return S3Helper.S3ClientInstance.GetPreSignedURL(req);
        }
    }

    public sealed class S3Helper
    {
        static AmazonS3 instance = null;
        static readonly object padlock = new object();

        S3Helper() { }

        public static AmazonS3 S3ClientInstance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = Amazon.AWSClientFactory.CreateAmazonS3Client(Helper.AppConfig.AWS_Id, Helper.AppConfig.AWS_Secret);
                    }

                    return instance;
                }
            }
        }
    }
}
