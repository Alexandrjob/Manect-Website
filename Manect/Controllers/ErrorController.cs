using Microsoft.AspNetCore.Mvc;

namespace Manect.Controllers
{
    public class ErrorController: Controller
    {
        public IActionResult Index(string errorMessage)
        {
            return View((object)errorMessage);
        }
    }
}
