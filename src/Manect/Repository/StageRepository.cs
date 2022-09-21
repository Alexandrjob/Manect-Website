using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manect.Controllers.Models;
using Manect.Data;
using Manect.Data.Entities;
using Manect.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manect.Repository
{
    public class StageRepository : IStageRepository
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

        public StageRepository(IServiceScopeFactory serviceScopeFactory, ILogger<StageRepository> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
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

            _logger.LogInformation(
                "Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}",
                DateTime.Now, userId, Status.Created, projectId, stage.Id);
        }

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

        public async Task DeleteStageAsync(int userId, int projectId, int stageId)
        {
            var dataContext = DataContext;
            var stage = await dataContext.Stages.FirstOrDefaultAsync(s => s.Id == stageId);
            stage.Status = Status.Deleted;
            _logger.LogInformation(
                "Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}",
                DateTime.Now, userId, Status.Deleted, stage.ProjectId, stage.Id);

            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task SetFlagValueAsync(int userId, int projectId, int stageId, Status status)
        {
            var dataContext = DataContext;

            _logger.LogInformation(
                "Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}",
                DateTime.Now, userId, status, projectId, stageId);

            var stage = await dataContext.Stages.FirstOrDefaultAsync(s => s.Id == stageId);
            stage.Status = status;
            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task EditStageAsync(Stage stage)
        {
            _logger.LogInformation(
                "Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}",
                DateTime.Now, stage.ExecutorId, Status.Modified, stage.ProjectId, stage.Id);

            var dataContext = DataContext;

            dataContext.Stages.Attach(stage);
            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
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
    }
}