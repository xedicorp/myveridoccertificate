using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Bal.Services;
using App.Entity.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using App.Foundation.Common;

namespace App.Bal.Repositories
{
    public class CaptchaService : ICaptchaService
    {
        private readonly IConfiguration _configuration;


        public CaptchaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HttpResponse> VerifyCaptcha(string token)
        {
            string? secretKey = _configuration.GetSection("GRecaptchaSecretKey").Value;

            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "secret", secretKey ?? "" }, { "response", token } });
            using (var http = new HttpClient())
            {
                HttpResponseMessage httpRecaptchaResponse = await http.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                string response = await httpRecaptchaResponse.Content.ReadAsStringAsync();
                if (!httpRecaptchaResponse.IsSuccessStatusCode)
                {
                    http.Dispose();
                    return new HttpResponse() { IsSuccess = false, Content = ErrorMessages.CaptchaVerificationFailed };
                }
                if (string.IsNullOrEmpty(response))
                {
                    http.Dispose();
                    return new HttpResponse() { IsSuccess = false, Content = ErrorMessages.InvalidRecaptchaResponse };
                }
                RecaptchaResponse? recaptchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(response);
                if (recaptchaResponse == null)
                {
                    http.Dispose();
                    return new HttpResponse() { IsSuccess = false, Content = ErrorMessages.InvalidRecaptchaResponse };
                }
                if (!recaptchaResponse.Success && recaptchaResponse.ErrorCodes != null)
                {
                    var errors = string.Join(",", recaptchaResponse.ErrorCodes);
                    http.Dispose();
                    return new HttpResponse() { IsSuccess = false, Content = errors };
                }
                if (recaptchaResponse.Score < 0.5)
                {
                    http.Dispose();
                    return new HttpResponse() { IsSuccess = false, Content = ErrorMessages.NotaBoat };
                }

            }

            return new HttpResponse() { IsSuccess = true, Content = "Captcha verification successful", StatusCode = 200 };
        }
    }
}
