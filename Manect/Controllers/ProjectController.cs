using Manect.Controllers.Models;
using Manect.Data.Entities;
using Manect.Interfaces;
using ManectTelegramBot.Models;
using ManectTelegramBot.Service;
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
        private readonly ServiceTelegramMessage _telegramMessage;

        public DataToChange DataToChange { get; set; }

        public ProjectController(IDataRepository dataRepository/*, ServiceTelegramMessage telegramMessage*/)
        {

            _dataRepository = dataRepository;
            //_telegramMessage = telegramMessage;
            DataToChange = new DataToChange();
        }

        public async Task<IActionResult> IndexAsync()
        {
            GetInformation();

            var project = await _dataRepository.GetProjectAllDataAsync(DataToChange.ProjectId);
            if (project == null)
            {
                return Redirect("/Error/Index");
            }

            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(DataToChange.CurrentUserId);
            return View(project);
        }

        public async Task<IActionResult> AddStageAsync()
        {
            GetInformation();

            await _dataRepository.AddStageAsync(DataToChange.CurrentUserId, DataToChange.ProjectId);
            return Redirect("Index");
        }

        public async Task<IActionResult> DeleteStageAsync(int stageId)
        {
            GetInformation();

            await _dataRepository.DeleteStageAsync(DataToChange.CurrentUserId, DataToChange.ProjectId, stageId);
            return Redirect("Index");
        }

        public async Task<IActionResult> DeleteProjectAsync()
        {
            GetInformation();

            await _dataRepository.DeleteProjectAsync(DataToChange.CurrentUserId, DataToChange.ProjectId);
            return RedirectToAction("Index", "Home");
        }

        public async Task SetFlagValueAsync(Status status, int stageId)
        {
            GetInformation();
            await _dataRepository.SetFlagValueAsync(DataToChange.CurrentUserId, DataToChange.ProjectId, stageId, status);
        }

        public async Task<IActionResult> ChengeExecutorAsync(int executorId, int stageId)
        {
            GetInformation();
            DataToChange.ExecutorId = executorId;
            DataToChange.StageId = stageId;
            await _dataRepository.EditExecutorAsync(DataToChange);

            MessageObject messageObject = await _dataRepository.GetInformationForMessageAsync(DataToChange);

            long chatId = await _dataRepository.GetTelegramIdAsync(DataToChange);
            await _telegramMessage.Send(chatId, messageObject);

            return Redirect("Index");
        }

        public async Task<IActionResult> ChengeExecutorDelegatedAsync(int executorId, int projectId, int stageId)
        {
            DataToChange.ExecutorId = executorId;
            DataToChange.ExecutorId = projectId;
            DataToChange.StageId = stageId;
            await _dataRepository.EditExecutorAsync(DataToChange);

            MessageObject messageObject = await _dataRepository.GetInformationForMessageAsync(DataToChange);

            long chatId = await _dataRepository.GetTelegramIdAsync(DataToChange);
            await _telegramMessage.Send(chatId, messageObject);

            return Redirect("StagesListDelegated");
        }

        public async Task<IActionResult> GetStageAsync([FromForm] Stage stage)
        {
            GetInformation();

            stage = await _dataRepository.GetStageAsync(stage.Id);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(DataToChange.CurrentUserId);
            return PartialView("StageForm", stage);
        }

        public async Task SaveStageAsync([FromForm] Stage stage)
        {
            GetInformation();
            await _dataRepository.EditStageAsync(stage);
        }

        public async Task<IActionResult> GetProjectAsync()
        {
            GetInformation();

            //TODO: Оптимизировать запросы.
            var project = await _dataRepository.GetProjectAllDataAsync(DataToChange.ProjectId);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(DataToChange.CurrentUserId);
            return PartialView("ProjectForm", project);
        }

        public async Task SaveProjectAsync([FromForm] Project project)
        {
            GetInformation();
            await _dataRepository.EditProjectAsync(project, DataToChange.CurrentUserId);
        }

        public async Task<IActionResult> GetFileListAsync([FromForm] int stageId)
        {
            GetInformation();
            DataToChange.StageId = stageId;
            List<AppFile> files = await _dataRepository.FileListAsync(DataToChange);
            return PartialView("File", files);
        }

        public async Task AddFileAsync([FromForm] int stageId, IList<IFormFile> Files)
        {
            GetInformation();
            DataToChange.StageId = stageId;
            DataToChange.Files = Files;

            await _dataRepository.AddFileAsync(DataToChange);
        }

        public async Task<IActionResult> DownloadFileAsync(int fileId)
        {
            GetInformation();
            DataToChange.FileId = fileId;
            AppFile file = await _dataRepository.GetFileAsync(DataToChange);

            return File(file.Content, file.Type, file.Name);
        }

        public async Task DeleteFileAsync([FromForm] int stageId, int fileId)
        {
            GetInformation();
            DataToChange.StageId = stageId;
            DataToChange.FileId = fileId;
            await _dataRepository.DeleteFileAsync(DataToChange);
        }

        public async Task<IActionResult> ProgectListExecutorsAsync()
        {
            GetInformation();
            bool isAdmin = await _dataRepository.IsAdminAsync(DataToChange);
            if (isAdmin)
            {
                var projects = await _dataRepository.GetProgectListExecutorsAsync();
                return View(projects);
            }
            return RedirectToAction("Index", "Error", new { errorMessage = "ТЕБЕ СЮДА НЕЛЬЗЯ ДРУЖОЧЕК-ПИРОЖОЧЕК" });
        }

        public async Task<IActionResult> StagesListDelegatedAsync()
        {
            GetInformation();

            var projects = await _dataRepository.GetStagesListDelegatedAsync(DataToChange);
            ViewBag.Executors = await _dataRepository.GetExecutorsToListExceptAsync(DataToChange.CurrentUserId);
            return View(projects);
        }

        public async Task<IActionResult> History()
        {
            GetInformation();
            List<HistoryItem> history = await _dataRepository.GetHistoryAsync();
            return View(history);
        }

        /// <summary>
        /// Записывает в обьект DataToChange CurrentUserId и ProjectId.
        /// </summary>
        private void GetInformation()
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId") |
                            HttpContext.Request.Cookies.ContainsKey("projectId"))
            {
                DataToChange.CurrentUserId = Convert.ToInt32(HttpContext.Request.Cookies["UserId"]);
                DataToChange.ProjectId = Convert.ToInt32(HttpContext.Request.Cookies["projectId"]);
            }
        }
    }
}
