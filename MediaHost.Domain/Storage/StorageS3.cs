using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace MediaHost.Domain.Storage
{
    public class StorageS3 : IStorage
    {
        public string StoreFile(Stream stream, string contentType)
        {
            string key = Guid.NewGuid().ToString("N") + "." + contentType.Split('/').LastOrDefault();

            Amazon.S3.Model.PutObjectRequest por = new Amazon.S3.Model.PutObjectRequest();
            por.WithBucketName(Helper.AppConfig.AWS_Bucket)
                .WithKey(key)
                .WithTimeout(3600000)
                .WithContentType(contentType)
                .WithStorageClass(Amazon.S3.Model.S3StorageClass.ReducedRedundancy)
                .WithCannedACL(Amazon.S3.Model.S3CannedACL.PublicRead)
                .WithInputStream(stream);

            Amazon.S3.Model.PutObjectResponse response = S3Helper.S3ClientInstance.PutObject(por);

            return key;
        }

        //public string StoreStreamingFile(Stream stream, string contentType)
        //{
        //    string key = Guid.NewGuid().ToString("N") + "." + contentType.Split('/').LastOrDefault();

            //if (stream.Length > 100000000)
            //{
            //    TransferUtility transferUtility = new TransferUtility(S3Helper.S3ClientInstance);
            //    TransferUtilityUploadRequest uploadRequest = new TransferUtilityUploadRequest();

            //    uploadRequest.WithBucketName(Helper.AppConfig.AWS_BucketStreaming)
            //        .WithKey(key)
            //        .WithCannedACL(Amazon.S3.Model.S3CannedACL.PublicRead)
            //        .WithAutoCloseStream(true)
            //        .WithContentType(contentType)
            //        .WithStorageClass(Amazon.S3.Model.S3StorageClass.ReducedRedundancy)
            //        .WithPartSize(10485760)
            //        .WithTimeout(3600000)
            //        .WithInputStream(stream);

            //    transferUtility.Upload(uploadRequest);
            //}
            //else
            //{
                //Amazon.S3.Model.PutObjectRequest por = new Amazon.S3.Model.PutObjectRequest();
                //por.WithBucketName(Helper.AppConfig.AWS_Bucket)
                //    .WithKey(key)
                //    .WithCannedACL(Amazon.S3.Model.S3CannedACL.PublicRead)
                //    .WithContentType(contentType)
                //    .WithStorageClass(Amazon.S3.Model.S3StorageClass.ReducedRedundancy)
                //    .WithTimeout(3600000)
                //    .WithInputStream(stream);

                //Amazon.S3.Model.PutObjectResponse response = S3Helper.S3ClientInstance.PutObject(por);
            //}

        //    return key;
        //}

        public string GetFileUrl(string key, bool isSSL)
        {
            DateTime expiration = DateTime.UtcNow.AddDays(1);

            Amazon.S3.Model.GetPreSignedUrlRequest req = new Amazon.S3.Model.GetPreSignedUrlRequest();
            req.WithBucketName(Helper.AppConfig.AWS_Bucket)
                .WithKey(key)
                .WithExpires(expiration)
                .WithVerb(Amazon.S3.Model.HttpVerb.GET);

            return S3Helper.S3ClientInstance.GetPreSignedURL(req);

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


        public bool RemoveFile(string key)
        {
            try
            {
                var delRequest = new Amazon.S3.Model.DeleteObjectRequest()
                    .WithBucketName(Helper.AppConfig.AWS_Bucket)
                    .WithKey(key);

                S3Helper.S3ClientInstance.DeleteObject(delRequest);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }

    public sealed class S3Helper
    {
        static AmazonS3 _s3ClientInstance = null;
        static readonly object padlock = new object();

        S3Helper() { }

        public static AmazonS3 S3ClientInstance
        {
            get
            {
                lock (padlock)
                {
                    if (_s3ClientInstance == null)
                    {
                        _s3ClientInstance = Amazon.AWSClientFactory.CreateAmazonS3Client(Helper.AppConfig.AWS_Id, Helper.AppConfig.AWS_Secret);
                    }

                    return _s3ClientInstance;
                }
            }
        }
    }
}
