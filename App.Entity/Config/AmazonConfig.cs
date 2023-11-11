using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Config
{
    public class AmazonConfig
    {
        public const string Path = "AmazonAWS";

        public string BucketFolderPrefix { get; set; } = string.Empty;
        public string AWSAccessKey { get; set; } = string.Empty;
        public string AWSSecretKey { get; set; } = string.Empty;
        public string AWSBucketName { get; set; } = string.Empty;
    }
}
