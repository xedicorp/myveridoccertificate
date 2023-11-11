using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using App.Bal.Services;
using App.Entity.Config;
using Amazon.S3;
using Amazon.Runtime;
using Amazon;
using Amazon.S3.Model;
using System.Net;
using App.Foundation.Common;

namespace App.Bal.Repositories
{
    public class StorageService : IStorageService, IDisposable
    {

        private readonly IConfiguration _configuration;
        private readonly AmazonConfig _amazonConfig;

        private readonly AmazonS3Client amazonS3Client;

        public string GetConnectionString => _configuration.GetConnectionString("MainAppConnection_Prod") ?? Utils.ConnectionStringProd;

        public string FolderPrefix => _amazonConfig.BucketFolderPrefix;

        public StorageService(IConfiguration configuration)
        {
            _amazonConfig = new ();
            _configuration = configuration;
            _configuration.GetSection(AmazonConfig.Path).Bind(_amazonConfig);
            var credentials = new BasicAWSCredentials(_amazonConfig.AWSAccessKey, _amazonConfig.AWSSecretKey);
            amazonS3Client = new AmazonS3Client(credentials, RegionEndpoint.APSoutheast2);
        }

        public async Task<bool> CreateBucket(string bucketName)
        {
            bool result = false;
            bool isExist = await IsBucketExist(bucketName);
            if (!isExist)
            {
                // Construct request
                PutBucketRequest request = new PutBucketRequest
                {
                    BucketName = bucketName,
                    BucketRegion = S3Region.APS2,         // set region to EU
                    CannedACL = S3CannedACL.PublicRead  // make bucket publicly readable
                };
                PutBucketResponse bucketResponse = await amazonS3Client.PutBucketAsync(request);
                if (bucketResponse != null && bucketResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }


        public async Task<bool> IsBucketExist(string bucketName)
        {
            ListBucketsResponse listBuckets = await amazonS3Client.ListBucketsAsync();
            if (listBuckets.HttpStatusCode == HttpStatusCode.OK)
            {
                return listBuckets.Buckets.FirstOrDefault(e => e.BucketName == bucketName) != null;
            }
            return false;
        }


        public void Dispose()
        {
            amazonS3Client.Dispose();
        }
          
        public async Task<bool> CreateFolder(string folderName)
        {
            PutObjectRequest request = new ()
            {
                BucketName = _amazonConfig.AWSBucketName,
                Key = folderName // <-- in S3 key represents a path  
            };

            PutObjectResponse response = await amazonS3Client.PutObjectAsync(request);
            return response != null && response.HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
