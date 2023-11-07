using Microsoft.AspNetCore.Mvc;

namespace SmartRealms.API.Controllers
{
    public class BaseApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
