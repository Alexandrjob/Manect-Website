using Manect.Data.Entities;
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
            //TODO: Как не копировать по триста раз эту хрень?
            //var name = HttpContext.User.Identity.Name;
            //var currentUserId = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

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
            var currentUserId = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.AddStageAsync(currentUserId, projectId);

            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            return View("Index", project);
        }

        public async Task<IActionResult> DeleteStageAsync(int stageId, int projectId)
        {
            var name = HttpContext.User.Identity.Name;
            var currentUserId = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.DeleteStageAsync(currentUserId, projectId, stageId);

            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            return View("Index", project);
        }

        public async Task<IActionResult> DeleteProjectAsync(int projectId)
        {
            var name = HttpContext.User.Identity.Name;
            var currentUserId = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.DeleteProjectAsync(currentUserId, projectId);

            return RedirectToAction("Index", "Manager");
        }

        public async Task<IActionResult> SetFlagValueAsync(Status status, int projectId, int stageId)
        {
            var name = HttpContext.User.Identity.Name;
            var currentUserId = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.SetFlagValueAsync(currentUserId, projectId, stageId, status);
            return Ok();
        }
    }
}
