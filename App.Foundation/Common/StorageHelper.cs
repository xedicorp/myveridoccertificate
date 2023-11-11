using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Foundation.Common
{
    public class StorageHelper
    {

        /// <summary>
        /// Create valid bucket name
        /// </summary>
        /// <param name="rawBucketName">raw bucket name</param>
        /// <returns>Return valid bucket name</returns>
        public static string CreateOrganisationBucketName(string rawBucketName)
        {
            return Regex.Replace(rawBucketName, @"[^0-9a-zA-Z]+", "");
        }

        public static string GetOrganisationName(string? email)
        {
            return Regex.Replace(email ?? "", @"@.*","");
        }
    }
}
