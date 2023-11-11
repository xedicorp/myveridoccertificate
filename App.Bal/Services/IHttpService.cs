using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entity.Models;

namespace App.Bal.Services
{
    public interface IHttpService
    {
        Task<HttpResponse> Get(string url);

        Task<HttpResponse> Post<T>(string url, T data);
    }
}
