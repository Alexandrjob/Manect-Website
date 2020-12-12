using Manect.Data.Entities;
using Manect.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Controllers
{
    [Authorize]
    public class ProjectController: Controller
    {
        private readonly IDataRepository _dataRepository;

        private int currentUserId;
        private int currentProjectId;

        public DataToChange DataToChange { get; set; }

        public ProjectController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
            DataToChange = new DataToChange();
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
            return Redirect("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStageAsync(int stageId)
        {
            GetInformation();

            await _dataRepository.DeleteStageAsync(currentUserId, currentProjectId, stageId);
            return Redirect("Index");
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
        public async Task<IActionResult> ChengeExecutorAsync(int executorId, int stageId)
        {
            GetInformation();

            await _dataRepository.ChengeExecutorAsync(executorId, currentProjectId, stageId);
            return Redirect("Index");
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

        public async Task<IActionResult> GetProjectAsync()
        {
            GetInformation();

            //TODO: Оптимизировать запросы.
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

        public async Task AddFileAsync([FromForm] int stageId, IList<IFormFile> Files)
        {
            GetInformation();
            DataToChange.StageId = stageId;
            DataToChange.Files = Files;

            await _dataRepository.AddFileAsync(DataToChange);
        }
        public async Task DownloadFileAsync([FromForm] int fileId)
        {
            GetInformation();
            DataToChange.FileId = fileId;

            AppFile a = await _dataRepository.GetFileAsync(DataToChange);
        }

        private void GetInformation()
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId") |
                            HttpContext.Request.Cookies.ContainsKey("projectId"))
            {
                currentUserId = Convert.ToInt32(HttpContext.Request.Cookies["UserId"]);
                currentProjectId = Convert.ToInt32(HttpContext.Request.Cookies["projectId"]);
            }
            DataToChange.UserId = currentUserId;
            DataToChange.ProjectId = currentProjectId;
        }
    }

}
