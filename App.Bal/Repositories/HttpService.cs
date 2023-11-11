using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App.Bal.Services;
using App.Entity.Models;
using Microsoft.Extensions.Configuration;

namespace App.Bal.Repositories
{
    public class HttpService : IHttpService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpService(IConfiguration configuration, IHttpClientFactory httpClient)
        {
            _configuration = configuration;
            _httpClientFactory = httpClient;
        }

        public async Task<HttpResponse> Get(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");

            HttpClient client = _httpClientFactory.CreateClient();

            HttpResponseMessage responseMessage = await client.SendAsync(request);
            string response = await responseMessage.Content.ReadAsStringAsync();

            return new HttpResponse()
            {
                StatusCode = (int)responseMessage.StatusCode,
                Content = response,
                IsSuccess = responseMessage.IsSuccessStatusCode
            };

        }


        public async Task<HttpResponse> Post<T>(string url, T data)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Accept", "application/json");

            StringContent content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            request.Content = content;

            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);

            string response = await responseMessage.Content.ReadAsStringAsync();

            return new HttpResponse()
            {
                StatusCode = (int)responseMessage.StatusCode,
                Content = response,
                IsSuccess = responseMessage.IsSuccessStatusCode
            };

        }
    }
}
