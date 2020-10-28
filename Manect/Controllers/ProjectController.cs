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
            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            if(project == null)
            {
                return Redirect("/Error/Index");
            }

            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUser.Id);
            return View(project);
        }

        public async Task<IActionResult> AddStageAsync(int projectId)
        {
            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.AddStageAsync(currentUser, projectId);

            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUser.Id);
            return View("Index", project);
        }

        public async Task<IActionResult> DeleteStageAsync(int stageId, int projectId)
        {
            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.DeleteStageAsync(currentUser.Id, projectId, stageId);

            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUser.Id);
            return View("Index", project);
        }

        public async Task<IActionResult> DeleteProjectAsync(int projectId)
        {
            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.DeleteProjectAsync(currentUser.Id, projectId);

            return RedirectToAction("Index", "Manager");
        }

        public async Task<IActionResult> SetFlagValueAsync(Status status, int projectId, int stageId)
        {
            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            await _dataRepository.SetFlagValueAsync(currentUser.Id, projectId, stageId, status);

            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUser.Id);
            return View("Index", project);
        }

        public async Task<IActionResult> ChengeExecutorAsync(int executorId, int projectId, int stageId)
        {
            await _dataRepository.ChengeExecutorAsync(executorId, projectId, stageId);

            var name = HttpContext.User.Identity.Name;
            var currentUser = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUser.Id);
            return View("Index", project);
        }

        [HttpPost]
        public async Task<IActionResult> GetStageAsync([FromForm] Stage stage)
        {
            int projectId = 1;
            int currentUserId = 1;
            var project = await _dataRepository.GetAllProjectDataAsync(projectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUserId);

            return PartialView("EditStageForm", project);
        }
    }
}
