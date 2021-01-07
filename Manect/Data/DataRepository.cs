using Manect.Data.Entities;
using Manect.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using ManectTelegramBot.Models;
using Manect.Controllers.Models;

namespace Manect.Data
{
    public partial class DataRepository: IDataRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        private ProjectDbContext DataContext
        {
            get
            {
                var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetService<ProjectDbContext>();
            }
        }

        public DataRepository(IServiceScopeFactory serviceScopeFactory, ILogger<DataRepository> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task<int> FindUserIdByNameOrDefaultAsync(string name)
        {
            return await DataContext.Executors
               .AsNoTracking()
               .Where(user => user.UserName == name)
               .Select(u => u.Id)
               .FirstOrDefaultAsync();
        }

        public async Task<int> FindUserIdByEmailOrDefaultAsync(string email)
        {
            return await DataContext.Executors
                .AsNoTracking()
                .Where(user => user.Email == email)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();
        }

        public async Task AddStageAsync(int userId, int projectId)
        {
            var user = new Executor
            {
                Id = userId
            };

            var dataContext = DataContext;
            dataContext.Executors.Attach(user);

            var stage = new Stage("Новый этап", user)
            {
                ProjectId = projectId
            };

            dataContext.Entry(stage).State = EntityState.Added;
            await dataContext.SaveChangesAsync();

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, userId, Status.Created, projectId, stage.Id);
        }

        public async Task AddProjectDefaultAsync(int CurrentUserId)
        {
            var user = new Executor();
            user.Id = CurrentUserId;

            Project project = new Project("Стандартный шаблон проекта", 0, user,
                new List<Stage>()
                {
                        new Stage("Встреча  с клиентом", user),
                        new Stage("Замер обьекта", user),
                        new Stage("Просчет", user),
                        new Stage("Эскиз", user),
                        new Stage("Материалы и счета", user),
                        new Stage("Монтаж", user),
                        new Stage("Нюансы проекта", user)
                });

            var dataContext = DataContext;
            dataContext.Entry(project).State = EntityState.Added;

            foreach (Stage stage in project.Stages)
            {
                dataContext.Entry(stage).State = EntityState.Added;
                _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, user.Id, Status.Created, project.Id, stage.Id);
            }

            await dataContext.SaveChangesAsync();

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} Проект {ProjectId}", DateTime.Now, user.Id, Status.Created, project.Id);
        }

        public async Task DeleteStageAsync(int userId, int projectId, int stageId)
        {
            var dataContext = DataContext;
            var stage = await dataContext.Stages.FirstOrDefaultAsync(s => s.Id == stageId);
            stage.Status = Status.Deleted;
            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, userId, Status.Deleted, stage.ProjectId, stage.Id);

            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int userId, int projectId)
        {
            var project = new Project
            {
                Id = projectId
            };
            var dataContext = DataContext;
            var stage = await dataContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} Проект: {ProjectId}", DateTime.Now, userId, Status.Deleted, project.Id);

            dataContext.Entry(project).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task SetFlagValueAsync(int userId, int projectId, int stageId, Status status)
        {
            var dataContext = DataContext;

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, userId, status, projectId, stageId);

            var stage = await dataContext.Stages.FirstOrDefaultAsync(s => s.Id == stageId);
            stage.Status = status;
            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        //TODO: Оптимизировать много лишних данных.
        public async Task<List<Project>> GetProjectOrDefaultToListAsync(int userId)
        {
            var projects = await DataContext.Projects
                .AsNoTracking()
                .Where(p => p.Executor.Id == userId && p.Status != Status.Deleted)
                .ToListAsync();
            if (projects != null)
            {
                return projects;
            }

            return new List<Project>();
        }

        public async Task<Project> GetProjectAllDataAsync(int projectId)
        {
            var dataContext = DataContext;
            //TODO:В будущем оптимизировать.
            var project = new Project();

            project = await dataContext.Projects
            .AsNoTracking()
            .Where(p => p.Id == projectId)
            //.Include(p => p.Stages.Where(s => s.Status != Status.Deleted)Тут доисать явную загрузку исполнителя)
            .Include(p => p.Executor)
            .FirstOrDefaultAsync();

            List<Stage> stages = new List<Stage>();

            stages = await dataContext.Stages
            .AsNoTracking()
            .Where(s => s.ProjectId == project.Id && s.Status != Status.Deleted)
            .Include(s => s.Executor)
            .OrderBy(s => s.CreationDate)
            .ToListAsync();

            var allStages = await dataContext.Stages
            .AsNoTracking()
            .Where(s => s.ProjectId == project.Id )
            .Include(s => s.Executor)
            .OrderBy(s => s.CreationDate)
            .ToListAsync();

            project.Stages = stages;
            return project;
        }

        //TODO: Оптимизировать.
        public async Task<Stage> GetStageAsync(int stageId)
        {
            var dataContext = DataContext;

            var stage = await dataContext.Stages
                .AsNoTracking()
                .Include(s => s.Executor)
                //.Where(s => s.Id == stageId)
                //.Select(stage => new
                //{
                //    stage,
                //    stage.Executor.Id,
                //    stage.Executor.FirstName,
                //    stage.Executor.LastName
                //})
                //.AsQueryable()
                //.Select(stage => new Stage
                //{

                //    Executor = new Executor()
                //    {
                //        Id = stage.Id,
                //        FirstName = stage.FirstName,
                //        LastName = stage.LastName
                //    }
                //})
                .FirstOrDefaultAsync(s => s.Id == stageId);

            return stage;
        }

        public async Task<List<Executor>> GetExecutorsToListExceptAsync(int userId)
        {
            var dataContext = DataContext;
            const string CREATORUSERNAME = "Sasha";

            List<Executor> executors = await dataContext.Executors
                .AsNoTracking()
                .Where(e => e.Id != userId && e.UserName != CREATORUSERNAME)
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName
                })
                .AsQueryable()
                .Select(un => new Executor
                {
                    Id = un.Id,
                    FirstName = un.FirstName,
                    LastName = un.LastName
                })
                .ToListAsync();

            return executors;
        }

        public async Task EditExecutorAsync(DataToChange dataToChange)
        {
            var dataContext = DataContext;

            var stage = await dataContext.Stages
                .FirstOrDefaultAsync(s => s.Id == dataToChange.StageId);

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, stage.ExecutorId, Status.Modified, stage.ProjectId, stage.Id);
            stage.ExecutorId = dataToChange.ExecutorId;
            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task EditStageAsync(Stage stage)
        {
            var dataContext = DataContext;
            //TODO: Почему то ругается на отсутствие полей, в проекте такого нет
            var oldStage = await dataContext.Stages
                .AsNoTracking()
                .Where(s => s.Id == stage.Id)
                .Select(u => new
                {
                    u.ProjectId
                })
                .AsQueryable()
                .Select(s => new Stage
                {
                    ProjectId = s.ProjectId
                })
                .FirstOrDefaultAsync();

            stage.ProjectId = oldStage.ProjectId;

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, stage.ExecutorId, Status.Modified, stage.ProjectId, stage.Id);

            dataContext.Stages.Attach(stage);
            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task EditProjectAsync(Project project, int userId)
        {
            var dataContext = DataContext;
            //TODO: ПЛОХО!!
            project.ExecutorId = userId;
            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} Проект {ProjectId}", DateTime.Now, userId, Status.Modified, project.Id);

            dataContext.Projects.Attach(project);
            dataContext.Entry(project).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task AddFileAsync(DataToChange dataToChange)
        {
            var dataContext = DataContext;
            foreach (var file in dataToChange.Files)
            {
                if (IsNotExtensionValid(file)) return;

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                if (memoryStream.Length > 5242880) return;

                var appFile = new AppFile()
                {
                    StageId = dataToChange.StageId,
                    Name = file.FileName,
                    Type = file.ContentType,
                    Content = memoryStream.ToArray()
                };
                dataContext.Files.Add(appFile);
                //TODO: Надеюсь, что будет незаметно.;
                await dataContext.SaveChangesAsync();
                _logger.LogInformation("Время: {TimeAction}. Пользователь(Id:{ExecutorId}) , {Status} файл(Id:{FileId}) в Проекте (Id:{ProjectId})", DateTime.Now, dataToChange.CurrentUserId, Status.Created, appFile.Id, dataToChange.ProjectId);
            }
        }

        private bool IsNotExtensionValid(IFormFile file)
        {
            var fileType = file.FileName.Split(".").Last();
            foreach (var extensions in Enum.GetValues(typeof(Extensions)))
            {
                if (fileType == extensions.ToString())
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<AppFile> GetFileAsync(DataToChange dataToChange)
        {
            _logger.LogInformation("Время: {TimeAction}. Пользователь(Id:{ExecutorId}) , {Status} файл(Id:{FileId}) в Проекте (Id:{ProjectId})",
                                 DateTime.Now, dataToChange.CurrentUserId, Status.Received, dataToChange.FileId, dataToChange.ProjectId);

            return await DataContext.Files.AsNoTracking().FirstOrDefaultAsync(f => f.Id == dataToChange.FileId);
        }

        public async Task<List<AppFile>> FileListAsync(DataToChange dataToChange)
        {
            var files = await DataContext.Files
                                         .Where(f => f.StageId == dataToChange.StageId)
                                         .Select(obj => new
                                         {
                                             obj.Id,
                                             obj.Name,
                                             obj.StageId,
                                             obj.Content.Length
                                         })
                                        .AsQueryable()
                                        .Select(f => new AppFile
                                        {
                                            Id = f.Id,
                                            Name = f.Name,
                                            StageId = f.StageId,
                                            Length = f.Length
                                        })
                                        .ToListAsync();

            return files;
        }

        public async Task DeleteFileAsync(DataToChange dataToChange)
        {
            var file = new AppFile
            {
                Id = dataToChange.FileId
            };
            _logger.LogInformation("Время: {TimeAction}. Пользователь(Id:{ExecutorId}) , {Status} файл(Id:{FileId}) в этапе (Id:{ProjectId})",
                                DateTime.Now, dataToChange.CurrentUserId, Status.Deleted, dataToChange.FileId, dataToChange.StageId);

            var dataContext = DataContext;
            dataContext.Files.Attach(file);
            dataContext.Entry(file).State = EntityState.Deleted;
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<Executor>> GetProgectListExecutorsAsync()
        {
            var dataContext = DataContext;
            const string ADMINUSERNAME = "Katya";
            const string CREATORUSERNAME = "Sasha";

            List<Executor> projectsExecutors = await dataContext.Executors
                .AsNoTracking()
                .Where(executor => executor.UserName != ADMINUSERNAME && executor.UserName != CREATORUSERNAME)
                .Select(executor => new
                {
                    executor.Id,
                    executor.FirstName,
                    executor.LastName,
                    Projects = executor.Projects.Select(project => new
                    {
                        project.Id,
                        project.Name
                    })

                })
                .AsQueryable()
                .Select(executor => new Executor
                {
                    Id = executor.Id,
                    FirstName = executor.FirstName,
                    LastName = executor.LastName,
                    Projects = executor.Projects.Select(project => new Project
                    {
                        Id = project.Id,
                        Name = project.Name
                    }).ToList()
                })
                .ToListAsync();

            return projectsExecutors;
        }

        public async Task<bool> IsAdminAsync(DataToChange dataToChange)
        {
            const string ADMINUSERNAME = "Katya";
            const string CREATORUSERNAME = "Sasha";
            var dataContext = DataContext;

            var currentUser = await dataContext.Executors.Where(executor => executor.Id == dataToChange.CurrentUserId).Select(ex => ex.UserName).FirstOrDefaultAsync();

            if (currentUser == ADMINUSERNAME || currentUser == CREATORUSERNAME)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Project>> GetStagesListDelegatedAsync(DataToChange dataToChange)
        {
            var dataContext = DataContext;
            var projects = await dataContext.Projects
                .AsNoTracking()
                .Where(p => p.ExecutorId != dataToChange.CurrentUserId)
                .Select(project => new
                {
                    project.Id,
                    project.Name,
                    project.Executor.FirstName,
                    project.Executor.LastName,
                    project.ExecutorId,
                    project.CreationDate,
                    Stages = project.Stages.Where(stage => stage.ExecutorId == dataToChange.CurrentUserId)
                })
                .AsQueryable()
                .Select(project => new Project
                {
                    Id = project.Id,
                    Name = project.Name,
                    CreationDate = project.CreationDate,
                    Executor = new Executor()
                    {
                        FirstName = project.FirstName,
                        LastName = project.LastName
                    },
                    ExecutorId = project.ExecutorId,
                    Stages = project.Stages.ToList()
                })
                .ToListAsync();

            projects = projects.Where(p => p.Stages.Count != 0).ToList();
            projects.ForEach(p => p.Stages = p.Stages.OrderBy(s => s.CreationDate).ToList());
            return projects;
        }

        public async Task<long> GetTelegramIdAsync(DataToChange dataToChange)
        {
            var telegramId = await DataContext.Executors.Where(executor => executor.Id == dataToChange.CurrentUserId)
                                                        .Select(executor => executor.TelegramId)
                                                        .FirstOrDefaultAsync();
            return telegramId;
        }

        public async Task AddTelegramIdAsync(DataToChange dataToChange)
        {
            var dataContext = DataContext;
            //TODO:Нормально нет.
            var currentExecutor = await dataContext.Executors.FirstOrDefaultAsync(executor => executor.Id == dataToChange.CurrentUserId);

            currentExecutor.TelegramId = dataToChange.TelegramId;
            dataContext.Entry(currentExecutor).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task<MessageObject> GetInformationForMessageAsync(DataToChange dataToChange)
        {
            var dataContext = DataContext;

            var messageObject = await dataContext.Executors
                .AsNoTracking()
                .Where(exe => exe.Id == dataToChange.CurrentUserId)
                .Select(exe => new
                {
                    exe.FirstName,
                    exe.LastName,
                    Project = exe.Projects
                                     .Where(project => project.Id == dataToChange.ProjectId)
                                     .Select(p => new
                                     {
                                         p.Name,
                                         p.Executor.FirstName,
                                         p.Executor.LastName,
                                         StageName = p.Stages.Where(stage => stage.Id == dataToChange.StageId).Select(s => s.Name).FirstOrDefault()
                                     })
                                     .FirstOrDefault()

                })
                .AsQueryable()
                .Select(messageObject => new MessageObject
                {
                    SenderExecutor = new SenderExecutor()
                    {
                        FirstName = messageObject.FirstName,
                        LastName = messageObject.LastName
                    },

                    StageName = messageObject.Project.StageName,
                    ProjectName = messageObject.Project.Name,

                    ExecutorProject = new ExecutorProject()
                    {
                        FirstName = messageObject.Project.FirstName,
                        LastName = messageObject.Project.LastName
                    }
                })
                .FirstOrDefaultAsync();

            var recipientExecutor = await dataContext.Executors
                .AsNoTracking()
                .Where(exe => exe.Id == dataToChange.ExecutorId)
                .Select(exe => new
                {
                    exe.FirstName,
                    exe.LastName,
                    exe.TelegramId
                })
                .AsQueryable()
                .Select(recipientExecutor => new RecipientExecutor
                {
                    FirstName = recipientExecutor.FirstName,
                    LastName = recipientExecutor.LastName,
                    TelegramId = recipientExecutor.TelegramId
                })
                .FirstOrDefaultAsync();

            messageObject.RecipientExecutor = recipientExecutor;
            return messageObject;
        }

        public async Task<Executor> GetFullNameAsync(DataToChange dataToChange)
        {
            var executor = await DataContext.Executors
               .AsNoTracking()
               .Where(exe => exe.Id == dataToChange.CurrentUserId)
               .Select(exe => new
               {
                   exe.FirstName,
                   exe.LastName
               })
               .AsQueryable()
               .Select(executor => new Executor
               {
                   FirstName = executor.FirstName,
                   LastName = executor.LastName
               })
               .FirstOrDefaultAsync();

            return executor;
        }

        public async Task<List<HistoryItem>> GetHistoryAsync()
        {
            var dataContext = DataContext;
            List<HistoryItem> history = new List<HistoryItem>();



            return history;
        }
    }
}