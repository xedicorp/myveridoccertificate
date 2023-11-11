using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App.Entity.Dto.Square
{
    public class SquareErrors
    {
        [JsonProperty("category")]
        public string Category { get; set; } = string.Empty;

        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        [JsonProperty("detail")]
        public string Detail { get; set; } = string.Empty;

        [JsonProperty("Field")]
        public string Field { get; set; } = string.Empty;

    }
}
