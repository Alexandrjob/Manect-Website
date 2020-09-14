using Manect.Identity;
using Manect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Manect.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IDataRepository _dataRepository;
        private readonly ISyncTables _syncTables;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDataRepository dataRepository,
            ISyncTables syncTables)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dataRepository = dataRepository;
            _syncTables = syncTables;
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
            return Redirect("/Admin/Index");
        }

        //[HttpPost]
        public async Task<IActionResult> AddProject()
        {
            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserByNameOrDefaultAsync(name);
            //var project = _dataRepository.ToListProjectsAsync(currentUser.Name).Result.FirstOrDefault();
            //await _dataRepository.AddStageAsync(currentUser, project);
            await _dataRepository.AddProjectDefaultAsync(currentUser);
            return Redirect("/Admin/Index");
        }
    }
}
