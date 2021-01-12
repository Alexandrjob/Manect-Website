using Manect.Controllers.Models;
using Manect.Identity;
using Manect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Manect.Controllers
{
    [Authorize]
    public class HomeController: Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataRepository _dataRepository;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDataRepository dataRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dataRepository = dataRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var name = HttpContext.User.Identity.Name;
            var currentUserId = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);
            HttpContext.Response.Cookies.Append("UserId", currentUserId.ToString());

            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUserId);
            return View(await _dataRepository.GetProjectOrDefaultToListAsync(currentUserId));
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("/Error/Index", new { errorMessage = "Данные пользователя для входа введены неверно"});
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("Index", "Error", new { errorMessage = "Пользователь не найден" });
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                //TODO: В будущем поменять на адаптивным метод, чтобы исключение не выкидывал а перекидывал на index
                return LocalRedirect("/Home/Index");
            }
            //TODO: Как отобразить ошибку пароля?
            return View(model);
        }

        public async Task<IActionResult> LogOffAsync()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> AddProject()
        {
            var name = HttpContext.User.Identity.Name;
            var currentUserId = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.AddProjectDefaultAsync(currentUserId);
            return Redirect("/Home/Index");
        }

        public IActionResult OpenProject(int projectId)
        {
            HttpContext.Response.Cookies.Append("projectId", projectId.ToString());
            return RedirectToAction("Index", "Project");
        }
    }
}
