using Manect.Identity;
using Manect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Manect.Controllers
{
    [Authorize]
    public class ManagerController: Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataRepository _dataRepository;

        public ManagerController(
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
            var currentUser = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUser.Id);
            return View(await _dataRepository.GetProjectOrDefaultToListAsync(currentUser.Id));
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
                return Redirect("/Error/Index");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Redirect("/Error/Index");
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
                //TODO: В будущем поменять на адаптивным метод, чтобы исключение не выкидывал а перекидывал на index
                return LocalRedirect(model.ReturnUrl);
            }
            return View(model);
        }

        public async Task<IActionResult> LogOffAsync()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Manager/Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddProject()
        {
            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.AddProjectDefaultAsync(currentUser);
            //TODO: нужно чтобы страница просто обновлялась.
            return Redirect("/Manager/Index");
        }

        public IActionResult OpenProject(int projectId)
        {
            HttpContext.Response.Cookies.Append("projectId", projectId.ToString());
            return RedirectToAction("Index", "Project"/*, new { projectId }*/);
        }
    }
}
