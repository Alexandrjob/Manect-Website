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

        public async Task<Executor> FindUserIdByNameOrDefaultAsync(string name)
        {
            return await DataContext.Executors
                .AsNoTracking()
                .Where(user => user.UserName == name)
                .Select(u => new
                {
                    u.Id
                })
                .AsQueryable()
                .Select(un => new Executor
                {
                    Id = un.Id
                })
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

        //TODO: сделать метод универсальным (получать значение по которому можно понять какой проект(шаблоны будут записаны в новой таблице) нужно создать)
        public async Task AddProjectDefaultAsync(Executor user)
        {
            Project project = new Project("Стандартный шаблон проекта", 0, user,
                new List<Stage>()
                {
                        new Stage("Обсуждение пожеланий клиента, предварительный эскиз", user),
                        new Stage("Замер обьекта", user),
                        new Stage("Окончательный эскиз ", user),
                        new Stage("Просчёт ", user),
                        new Stage("Дополнительные комплектующие и нюансы", user),
                        new Stage("Производство", user),
                        new Stage("Монтаж", user),
                        new Stage("Сдача объекта", user)
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
            var stage = new Stage
            {
                Id = stageId,
            };
            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, userId, Status.Deleted, projectId, stage.Id);

            var dataContext = DataContext;

            dataContext.Stages.Attach(stage);
            dataContext.Entry(stage).State = EntityState.Deleted;
            await dataContext.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int userId, int projectId)
        {
            var project = new Project
            {
                Id = projectId
            };

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} Проект: {ProjectId}", DateTime.Now, userId, Status.Deleted, project.Id);

            var dataContext = DataContext;
            dataContext.Projects.Attach(project);
            dataContext.Entry(project).State = EntityState.Deleted;
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

        public async Task<List<Project>> GetProjectOrDefaultToListAsync(int userId)
        {
            var projects = await DataContext.Projects
                .AsNoTracking()
                .Where(p => p.Executor.Id == userId)
                .ToListAsync();
            if (projects != null)
            {
                return projects;
            }

            return new List<Project>();
        }

        public async Task<Project> GetAllProjectDataAsync(int projectId, int stageId = default)
        {
            var dataContext = DataContext;
            //TODO:В будущем оптимизировать.
            var project = await dataContext.Projects
                .AsNoTracking()
                .Where(p => p.Id == projectId)
                .Include(p => p.Executor)
                .FirstOrDefaultAsync();

            List<Stage> stages = new List<Stage>();
            if (stageId == default)
            {
                stages = await dataContext.Stages
                .AsNoTracking()
                .Where(s => s.ProjectId == project.Id)
                .Include(s => s.Executor)
                .OrderBy(s => s.CreationDate)
                .ToListAsync();
            }
            else
            {

                var stage = await dataContext.Stages
                .AsNoTracking()
                .Where(s => s.ProjectId == project.Id)
                .Include(s => s.Executor)
                .FirstOrDefaultAsync(s => s.Id == stageId);

                stages.Add(stage);
            }


            project.Stages = stages;
            return project;
        }

        public async Task<List<Executor>> GetExecutorsToListExceptAsync(int userId)
        {
            var dataContext = DataContext;
            List<Executor> executors = await dataContext.Executors
                .AsNoTracking()
                .Where(e => e.Id != userId)
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

        public async Task ChengeExecutorAsync(int userId, int projectId, int stageId)
        {
            var dataContext = DataContext;

            var stage = await dataContext.Stages
                .FirstOrDefaultAsync(s => s.Id == stageId);

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, stage.ExecutorId, Status.Modified, stage.ProjectId, stage.Id);
            stage.ExecutorId = userId;
            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task ChangeStageAsync(Stage stage)
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

        public async Task ChangeProjectAsync(Project project, int userId)
        {
            var dataContext = DataContext;

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
                //TODO: Надеюсь, что будет незаметно.
                await dataContext.SaveChangesAsync();
                _logger.LogInformation("Время: {TimeAction}. Пользователь(Id:{ExecutorId}) , {Status} файл(Id:{FileId}) в Проекте (Id:{ProjectId})", DateTime.Now, dataToChange.UserId, Status.Created, appFile.Id, dataToChange.ProjectId);
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
            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} файл {FileId} в Проекте {ProjectId}", DateTime.Now, dataToChange.UserId, Status.Received, dataToChange.FileId, dataToChange.ProjectId);
            return await DataContext.Files.FirstOrDefaultAsync(f => f.Id == dataToChange.FileId);
        }

        public async Task<List<AppFile>> FileListAsync(DataToChange dataToChange)
        {
            var files = await DataContext.Files
                                         .Where(f => f.StageId == dataToChange.StageId)
                                         .Select(obj => new
                                         {
                                             obj.Id,
                                             obj.Name,
                                             obj.Content.Length
                                         })
                                        .AsQueryable()
                                        .Select(un => new AppFile
                                        {
                                            Id = un.Id,
                                            Name = un.Name,
                                            Length = un.Length
                                        })
                                        .ToListAsync();

            return files;
        }

        public Task DeleteFileAsync(DataToChange dataToChange)
        {
            throw new NotImplementedException();
        }
    }
}