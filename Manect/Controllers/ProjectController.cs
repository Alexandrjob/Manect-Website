using Manect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Manect.Controllers
{
    [Authorize]
    public class ProjectController: Controller
    {
        private readonly IDataRepository _dataRepository;

        public ProjectController(IDataRepository dataRepository)
        {
            
            _dataRepository = dataRepository;
        }

        public async Task<IActionResult> IndexAsync(int projectId)
        {
            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            if(project == null)
            {
                return Redirect("/Error/Index");
            }
            return View(project);
        }

        public async Task<IActionResult> AddStageAsync(int projectId)
        {
            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserByNameOrDefaultAsync(name);
            await _dataRepository.AddStageAsync(currentUser, projectId);

            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            return View("Index", project);
        }

        public async Task<IActionResult> DeleteStageAsync(int stageId, int projectId)
        {
            await _dataRepository.DeleteStageAsync(stageId);

            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            return View("Index", project);
        }
    }
}
