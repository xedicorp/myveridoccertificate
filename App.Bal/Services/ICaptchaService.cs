using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entity.Models;

namespace App.Bal.Services
{
    public interface ICaptchaService
    {
        public Task<HttpResponse> VerifyCaptcha(string token);
    }
}
