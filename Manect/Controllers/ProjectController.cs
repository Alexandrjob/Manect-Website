using Manect.Data.Entities;
using Manect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Manect.Controllers
{
    [Authorize]
    public class ProjectController: Controller
    {
        private readonly IDataRepository _dataRepository;

        private int currentUserId;
        private int currentProjectId;

        public ProjectController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task<IActionResult> IndexAsync()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("UserId") /*&
                            !HttpContext.Request.Cookies.ContainsKey("projectId")*/)
            {
                var name = HttpContext.User.Identity.Name;
                var currentUser = await _dataRepository.FindUserIdByNameOrDefaultAsync(name);

                HttpContext.Response.Cookies.Append("UserId", currentUser.Id.ToString());
                //HttpContext.Response.Cookies.Append("projectId", projectId.ToString());
            }
            GetInformation();

            var project = await _dataRepository.GetAllProjectDataAsync(currentProjectId);
            if (project == null)
            {
                return Redirect("/Error/Index");
            }

            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUserId);
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> AddStageAsync()
        {
            GetInformation();

            await _dataRepository.AddStageAsync(currentUserId, currentProjectId);

            var project = await _dataRepository.GetAllProjectDataAsync(currentProjectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUserId);
            return View("Index", project);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStageAsync(int stageId)
        {
            GetInformation();

            await _dataRepository.DeleteStageAsync(currentUserId, currentProjectId, stageId);

            var project = await _dataRepository.GetAllProjectDataAsync(currentProjectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUserId);
            return View("Index", project);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProjectAsync()
        {
            GetInformation();

            await _dataRepository.DeleteProjectAsync(currentUserId, currentProjectId);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task SetFlagValueAsync(Status status, int stageId)
        {
            GetInformation();
            await _dataRepository.SetFlagValueAsync(currentUserId, currentProjectId, stageId, status);
        }

        [HttpPost]
        public async Task<ViewResult> ChengeExecutorAsync(int executorId, int stageId)
        {
            GetInformation();

            await _dataRepository.ChengeExecutorAsync(executorId, currentProjectId, stageId);

            var project = await _dataRepository.GetAllProjectDataAsync(currentProjectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUserId);
            return View("Index", project);
        }

        [HttpPost]
        public async Task<IActionResult> GetStageAsync([FromForm] Stage stage)
        {
            GetInformation();

            var project = await _dataRepository.GetAllProjectDataAsync(currentProjectId, stage.Id);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUserId);
            return PartialView("StageForm", project);
        }

        [HttpPost]
        public async Task SaveStageAsync([FromForm] Stage stage)
        {
            GetInformation();
            await _dataRepository.ChangeStageAsync(stage);
        }

        public async Task<IActionResult> GetProjectAsync([FromForm] Project pr)
        {
            GetInformation();

            var project = await _dataRepository.GetAllProjectDataAsync(currentProjectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(currentUserId);
            return PartialView("ProjectForm", project);
        }
        [HttpPost]
        public async Task SaveProjectAsync([FromForm] Project project)
        {
            GetInformation();
            await _dataRepository.ChangeProjectAsync(project, currentUserId);
        }

        private void GetInformation()
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId") |
                            HttpContext.Request.Cookies.ContainsKey("projectId"))
            {
                currentUserId = Convert.ToInt32(HttpContext.Request.Cookies["UserId"]);
                currentProjectId = Convert.ToInt32(HttpContext.Request.Cookies["projectId"]);
            }
        }
    }
}
