using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using static System.Net.WebRequestMethods;
using VeriDocCertificate.CofoundaryCMS.Models;
using VeriDocCertificate.CofoundaryCMS.App_Data;
using System.Net;

namespace VeriDocCertificate.CofoundaryCMS.Controllers
{


    public class AccountController : Controller
    {

        private readonly string _myApi;

        public AccountController(IConfiguration configuration)
        {
            _myApi = configuration["ApiUrl"];

        }


        [HttpGet]
        public IActionResult signup([FromQuery(Name = "email")] string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                ViewBag.Email = email;
                return View();
            }
            else
            {
                return Redirect("~/404");
            }

        }


        [HttpPost]
        public IActionResult RequestTrial(string FreeTrialEmail)
        {
            if (!string.IsNullOrEmpty(FreeTrialEmail))
            {

                var returnurl = "~/account/signup?email=" + FreeTrialEmail;

                return Redirect(returnurl);
            }
            return Redirect("~/404/");

        }


        [HttpGet]
        public IActionResult verify([FromQuery(Name = "email")] string email, [FromQuery(Name = "hash")] string hash)
        {
            if (!string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(hash))
            {
                return View();
            }
            else
            {
                return Redirect("~/404");
            }

        }
        

        [HttpPost]
        public async Task<IActionResult> SubscribeUser([FromBody] SignUpModel signUp)
        {
            if (string.IsNullOrEmpty(signUp.FirstName))
            {
                return Json(new { code = 400, msg = "First Name is required!" });
            }
            if (string.IsNullOrEmpty(signUp.Email) || !Common.IsValidEmail(signUp.Email))
            {
                return Json(new { code = 400, msg = "Email is invalid!" });
            }
            if (string.IsNullOrEmpty(signUp.Plan) || (signUp.Plan.ToLower() != "standard" && signUp.Plan.ToLower() != "pro"))
            {
                return Json(new { code = 400, msg = "Plan is invalid" });
            }
            if (string.IsNullOrEmpty(signUp.PlanTimeSpan) || (signUp.PlanTimeSpan.ToLower() != "monthly" && signUp.PlanTimeSpan.ToLower() != "yearly"))
            {
                return Json(new { code = 400, msg = "Plan cadence is invalid" });
            }
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(200);
                    StringContent stringContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(signUp), Encoding.UTF8, "application/json");
                    HttpResponseMessage responseMessage = await httpClient.PostAsync($"{_myApi}/Subscribe/sign-up", stringContent);
                    string res = await responseMessage.Content.ReadAsStringAsync();
                    if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                    {
                        HttpResponseModel model = JsonConvert.DeserializeObject<HttpResponseModel>(res);
                        return Json(new { code = 400, msg = model.Content });
                    }
                    if (responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        
                        TempData["signupdata"] = res;
                        return Json(new { code = 200 });
                    }
                    if (responseMessage.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        return Json(new { code = 500, msg = await responseMessage.Content.ReadAsStringAsync() });
                    }
                }
            }
            catch(Exception){}
            return Json(new { code = 500 });
        }



    }
    
}
