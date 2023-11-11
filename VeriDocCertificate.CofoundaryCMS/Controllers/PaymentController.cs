using Microsoft.AspNetCore.Mvc;
using VeriDocCertificate.CofoundaryCMS.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace VeriDocCertificate.CofoundaryCMS.Controllers
{
    public class PaymentController : Controller
    {


        private readonly string _myApi;

        public PaymentController(IConfiguration configuration)
        {
            _myApi = configuration["ApiUrl"];

        }

        public IActionResult Index()
        {
            try
            {
                string res = TempData["signupdata"]?.ToString();
                if (!string.IsNullOrEmpty(res))
                {
                    SignupResponseModel model = JsonConvert.DeserializeObject<SignupResponseModel>(res);
                    return View(model);
                }
            }
            catch (Exception)
            {
                
            }
            return Redirect("~/404");
        }


        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody]SubscriptionResponse subscription)
        {
            string mess = string.Empty;
            try
            {
                using (HttpClient client = new ())
                {
                    client.Timeout = TimeSpan.FromSeconds(200);
                    StringContent stringContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(subscription), Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync($"{_myApi}/Square/create-subscription", stringContent);
                    if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                    {
                        HttpResponseModel model = JsonConvert.DeserializeObject<HttpResponseModel>(await responseMessage.Content.ReadAsStringAsync());
                        return Json(new { code = 400, msg = model.Content });
                    }
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { code = 200 });
                    }
                    if (responseMessage.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        return Json(new { code = 500, msg = await responseMessage.Content.ReadAsStringAsync() });
                    }
                }
            }
            catch (Exception e)
            {
                mess = e.Message + $"{Environment.NewLine}" + e.StackTrace;
            }
            return Json(new { code = 500, msg = mess });
        } 


        public IActionResult Success()
        {
            return View();
        }


        public async Task<IActionResult> Subscribe(string uid, string plan, string timespan)
        {
            try
            {
                PlanDetailsModel planDetailsModel = new () { PlanTimespan = timespan, CustomerId = uid, PlanName = plan };
                using HttpClient client = new();
                client.Timeout = TimeSpan.FromSeconds(200);
                StringContent stringContent = new(System.Text.Json.JsonSerializer.Serialize(planDetailsModel), Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await client.PostAsync($"{_myApi}/Subscribe/validate-customer", stringContent);
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    SignupResponseModel model = JsonConvert.DeserializeObject<SignupResponseModel>(await responseMessage.Content.ReadAsStringAsync());
                    return View(model);
                }
            }
            catch (Exception)
            {
            }
            return Redirect("~/404");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSubscription([FromBody] SubscriptionResponse subscription)
        {
            string mess = string.Empty;
            try
            {
                using (HttpClient client = new())
                {
                    client.Timeout = TimeSpan.FromSeconds(200);
                    StringContent stringContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(subscription), Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await client.PostAsync($"{_myApi}/Subscribe/update-subscription", stringContent);
                    if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                    {
                        HttpResponseModel model = JsonConvert.DeserializeObject<HttpResponseModel>(await responseMessage.Content.ReadAsStringAsync());
                        return Json(new { code = 400, msg = model.Content });
                    }
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        return Json(new { code = 200 });
                    }
                    if (responseMessage.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        return Json(new { code = 500, msg = await responseMessage.Content.ReadAsStringAsync() });
                    }
                }
            }
            catch (Exception e)
            {
                mess = e.Message + $"{Environment.NewLine}" + e.StackTrace;
            }
            return Json(new { code = 500, msg = mess });
        }
    }
}
