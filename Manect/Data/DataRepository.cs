using Manect.Data.Entities;
using Manect.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Data
{
    public class DataRepository : IDataRepository
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

        public async Task<ExecutorUser> FindUserByNameOrDefaultAsync(string name)
        {
            return await DataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Name == name);
        }

        public async Task<ExecutorUser> FindUserByEmailOrDefaultAsync(string email)
        {
            return await DataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Email == email);
        }

        //TODO: Вытаскивать проект с именем пользователя.
        public Task<Project> FindProjectAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Stage> AddStageAsync(ExecutorUser user, Project project)
        {
            var stage = new Stage("Новый этап", user)
            {
                ProjectId = project.Id
            };

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorName}, {Status} в Проект {ProjectName} новый Этап: {StageName}", DateTime.Now, user.Name, Status.Created, project.Name, stage.Name);

            var dataContext = DataContext;
            dataContext.Entry(stage).State = EntityState.Added;
            await dataContext.SaveChangesAsync();

            if (dataContext.Stages.FirstOrDefaultAsync(s => s.Id == stage.Id) == null)
            {
                _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorName}, НЕ СМОГ {Status} в Проект {ProjectName} новый Этап: {StageName}", DateTime.Now, user.Name, Status.Created, project.Name, stage.Name);
            }

            return stage;
        }

        //TODO: сделать метод универсальным (получать значение по которому можно понять какой проект(шаблоны будут записаны в новой таблице) нужно создать)
        public async Task<Project> AddProjectDefaultAsync(ExecutorUser user)
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
                dataContext.Entry(stage).State = EntityState.Added;
            await dataContext.SaveChangesAsync();

            //TODO: Сделать так во всех методах.
            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} новый Проект: {ProjectId}", DateTime.Now, user.Id, Status.Created, project.Id);

            var result = await dataContext.FurnitureProjects.FirstOrDefaultAsync(p => p.Id == project.Id);
            if (result == null)
            {
                _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} новый Проект: {ProjectId}", DateTime.Now, user.Id, Status.NotAdded, project.Id);
            }

            return project;
        }
        public async Task DeleteStageAsync(Stage stage)
        {
            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorName}, {Status} в Проекте {ProjectName} Этап: {StageName}", DateTime.Now, stage.Executor.Name, Status.Deleted, stage.Name);

            var dataContext = DataContext;
            dataContext.Stages.Remove(stage);
            await dataContext.SaveChangesAsync();

            var result = await dataContext.Stages.FirstOrDefaultAsync(s => s.Id == stage.Id);
            if (result != null)
            {
                _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorName}, НЕ СМОГ {Status} в Проекте {ProjectName} Этап: {StageName}", DateTime.Now, stage.Executor.Name, Status.NotDeleted, stage.ProjectId, stage.Name);
            }
        }

        public async Task DeleteProjectAsync(Project project)
        {
            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorName}, {Status} Проект: {ProjectName}", DateTime.Now, "Имя пользователя"/*project.Executor.Name*/, Status.Deleted, project.Name);
            var dataContext = DataContext;
            dataContext.Entry(project).State = EntityState.Deleted;
            await dataContext.SaveChangesAsync();

            if (dataContext.FurnitureProjects.FirstOrDefaultAsync(s => s.Id == project.Id).Result != null)
            {
                _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorName}, НЕ СМОГ {Status} Проект: {ProjectName}", DateTime.Now, "Имя пользователя"/*project.Executor.Name*/, Status.Deleted, project.Name);
            }
        }

        //TODO: Залогировать.
        public async Task SetFlagValueAsync(Stage stage, Status status)
        {
            stage.Status = status;
            var dataContext = DataContext;
            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<Project>> ToListProjectOrDefaultAsync(string userName)
        {
            var projects = await DataContext.FurnitureProjects.Where(p => p.Executor.Name == userName).ToListAsync();
            if (projects != null)
            {
                return projects;
            }

            return new List<Project>();
        }
    }
}
