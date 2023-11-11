using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Models
{
    public class HttpResponse
    {
        public int StatusCode { get; set; }
        public object Content { get; set; }
        public bool IsSuccess { get; set; }
    }
}
