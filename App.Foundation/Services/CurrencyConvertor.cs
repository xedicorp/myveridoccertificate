using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace App.Foundation.Services
{
    public class CurrencyConvertor
    {
        public static async Task<double> GetAUD()
        {
            double USDinAUD;
            try
            {
                HttpClient httpClient = new HttpClient();
                using HttpResponseMessage response = await httpClient.GetAsync("https://webjob.veridocsign.com/Currency/GetDefaultUSDtoAUD");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                dynamic data = JObject.Parse(responseBody);
                USDinAUD = data.success;
            }
            catch (HttpRequestException)
            {
                USDinAUD = 1.30;
            }

            return USDinAUD;
        }
    }
}
