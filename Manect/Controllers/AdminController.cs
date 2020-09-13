using Manect.Identity;
using Manect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Manect.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataRepository _dataRepository;

        public AdminController(
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
            return View(await _dataRepository.ToListProjectsAsync(name));
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
                return Redirect(model.ReturnUrl);
            }
            return View(model);
        }

        public async Task<IActionResult> LogOffAsync()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public async void AddProject()
        {
            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserByNameOrDefaultAsync(name);
            await _dataRepository.AddProjectDefaultAsync(currentUser);
        }
    }
}
