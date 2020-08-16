using Microsoft.AspNetCore.Mvc;

namespace DomaMebelSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
