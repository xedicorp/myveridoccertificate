using Microsoft.AspNetCore.Mvc;

namespace VeriDocCertificate.CofoundaryCMS.Controllers
{
    public class ErrorController : Controller
    {
        [Route("404")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
