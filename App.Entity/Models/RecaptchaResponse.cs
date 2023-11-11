using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Entity.Models
{
    public class RecaptchaResponse
    {

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("score")]
        public float Score { get; set; }

        [JsonProperty("error_codes")]
        public string[] ErrorCodes { get; set; }
    }
}
