using Microsoft.AspNetCore.Mvc;

namespace Manect.Controllers
{
    public class ErrorController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
