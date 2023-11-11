using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Bal.Services
{
    public interface IStorageService
    {
        public string GetConnectionString { get; }
        public string FolderPrefix { get; }

        public Task<bool> CreateBucket(string bucketName);
        public Task<bool> IsBucketExist(string bucketName);
        public Task<bool> CreateFolder(string folderName);
        
    }
}
