using System.Threading.Tasks;
using Manect.Identity;
using Manect.Interfaces.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Manect.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IExecutorRepository _executorRepository;
        private readonly IProjectRepository _projectRepository;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IExecutorRepository executorRepository,
            IProjectRepository projectRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _executorRepository = executorRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var name = HttpContext.User.Identity.Name;
            var currentUserId = await _executorRepository.FindUserIdByNameOrDefaultAsync(name);
            HttpContext.Response.Cookies.Append("UserId", currentUserId.ToString());

            ViewBag.Executors = await _executorRepository.GetExecutorsToListExceptAsync(currentUserId);
            return View(await _projectRepository.GetProjectOrDefaultToListAsync(currentUserId));
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
            // if (!ModelState.IsValid)
            // {
            //     return RedirectToAction($"/Error/Index");
            // }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("Index", "Error", new {errorMessage = "Пользователь не найден"});
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                //TODO: В будущем поменять на адаптивным метод, чтобы исключение не выкидывал а перекидывал на index
                return LocalRedirect("/Home/Index");
            }

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
            var currentUserId = await _executorRepository.FindUserIdByNameOrDefaultAsync(name);

            await _projectRepository.AddProjectDefaultAsync(currentUserId);
            return Redirect("/Home/Index");
        }

        public IActionResult OpenProject(int projectId)
        {
            HttpContext.Response.Cookies.Append("projectId", projectId.ToString());
            return RedirectToAction("Index", "Project");
        }
    }
}