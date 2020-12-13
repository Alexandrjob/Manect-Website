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

            var project = await _dataRepository.GetAllProjectDataAsync(DataToChange.ProjectId);
            if (project == null)
            {
                return Redirect("/Error/Index");
            }

            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(DataToChange.UserId);
            return View(project);
        }

        public async Task<IActionResult> AddStageAsync()
        {
            GetInformation();

            await _dataRepository.AddStageAsync(DataToChange.UserId, DataToChange.ProjectId);
            return Redirect("Index");
        }

        public async Task<IActionResult> DeleteStageAsync(int stageId)
        {
            GetInformation();

            await _dataRepository.DeleteStageAsync(DataToChange.UserId, DataToChange.ProjectId, stageId);
            return Redirect("Index");
        }

        public async Task<IActionResult> DeleteProjectAsync()
        {
            GetInformation();

            await _dataRepository.DeleteProjectAsync(DataToChange.UserId, DataToChange.ProjectId);
            return RedirectToAction("Index", "Home");
        }

        public async Task SetFlagValueAsync(Status status, int stageId)
        {
            GetInformation();
            await _dataRepository.SetFlagValueAsync(DataToChange.UserId, DataToChange.ProjectId, stageId, status);
        }

        public async Task<IActionResult> ChengeExecutorAsync(int executorId, int stageId)
        {
            GetInformation();

            await _dataRepository.ChengeExecutorAsync(executorId, DataToChange.ProjectId, stageId);
            return Redirect("Index");
        }

        public async Task<IActionResult> GetStageAsync([FromForm] Stage stage)
        {
            GetInformation();

            var project = await _dataRepository.GetAllProjectDataAsync(DataToChange.ProjectId, stage.Id);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(DataToChange.UserId);
            return PartialView("StageForm", project);
        }

        public async Task SaveStageAsync([FromForm] Stage stage)
        {
            GetInformation();
            await _dataRepository.ChangeStageAsync(stage);
        }

        public async Task<IActionResult> GetProjectAsync()
        {
            GetInformation();

            //TODO: Оптимизировать запросы.
            var project = await _dataRepository.GetAllProjectDataAsync(DataToChange.ProjectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(DataToChange.UserId);
            return PartialView("ProjectForm", project);
        }

        public async Task SaveProjectAsync([FromForm] Project project)
        {
            GetInformation();
            await _dataRepository.ChangeProjectAsync(project, DataToChange.UserId);
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

        public async Task<IActionResult> GetFileListAsync([FromForm] int stageId)
        {
            GetInformation();
            DataToChange.StageId = stageId;

            List<AppFile> files = await _dataRepository.FileListAsync(DataToChange);
            return View(files);
        }

        private void GetInformation()
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId") |
                            HttpContext.Request.Cookies.ContainsKey("projectId"))
            {
                DataToChange.UserId = Convert.ToInt32(HttpContext.Request.Cookies["UserId"]);
                DataToChange.ProjectId = Convert.ToInt32(HttpContext.Request.Cookies["projectId"]);
            }
        }
    }
}
