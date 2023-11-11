using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Models
{
    public class ErrorModel
    {
        public string? ErrorMessage { get; set; }
        public string? StackTrace { get; set; }
        public string? InnerErrorMessage { get; set; }
    }
}
