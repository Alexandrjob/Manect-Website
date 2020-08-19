using Microsoft.AspNetCore.Mvc;

namespace DomaMebelSite.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
