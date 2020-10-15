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
    public class DataRepository: IDataRepository
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
            return await DataContext.ExecutorUsers.Where(user => user.Name == name).Select(u => u.Id).FirstOrDefaultAsync();
        }

        public async Task<Executor> FindUserByEmailOrDefaultAsync(string email)
        {
            return await DataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Email == email);
        }

        //TODO: Вытаскивать проект с именем пользователя.
        public Task<Project> FindProjectAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Stage> AddStageAsync(int userId, int projectId)
        {
            var stage = new Stage("Новый этап", userId)
            {
                ProjectId = projectId
            };

            var dataContext = DataContext;
            dataContext.Entry(stage).State = EntityState.Added;
            await dataContext.SaveChangesAsync();

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, userId, Status.Created, projectId, stage.Id);

            //if (dataContext.Stages.FirstOrDefaultAsync(s => s.Id == stage.Id) == null)
            //{
            //    _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проект {ProjectId} новый Этап: {StageId}", DateTime.Now, user.Id, Status.NotAdded, projectId, stage.Id);
            //}

            return stage;
        }

        //TODO: сделать метод универсальным (получать значение по которому можно понять какой проект(шаблоны будут записаны в новой таблице) нужно создать)
        public async Task<Project> AddProjectDefaultAsync(int userId)
        {
            Project project = new Project("Стандартный шаблон проекта", 0, userId,
                new List<Stage>()
                {
                        new Stage("Обсуждение пожеланий клиента, предварительный эскиз", userId),
                        new Stage("Замер обьекта", userId),
                        new Stage("Окончательный эскиз ", userId),
                        new Stage("Просчёт ", userId),
                        new Stage("Дополнительные комплектующие и нюансы", userId),
                        new Stage("Производство", userId),
                        new Stage("Монтаж", userId),
                        new Stage("Сдача объекта", userId)
                });

            var dataContext = DataContext;
            dataContext.Entry(project).State = EntityState.Added;

            foreach (Stage stage in project.Stages)
                dataContext.Entry(stage).State = EntityState.Added;
            await dataContext.SaveChangesAsync();

            //TODO: Сделать так во всех методах.
            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} Проекте: {ProjectId}", DateTime.Now, userId, Status.Created, project.Id);

            //var result = await dataContext.FurnitureProjects.FirstOrDefaultAsync(p => p.Id == project.Id);
            //if (result == null)
            //{
            //    _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} новый Проект: {ProjectId}", DateTime.Now, user.Id, Status.NotAdded, project.Id);
            //}

            return project;
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
            dataContext.Stages.Remove(stage);
            await dataContext.SaveChangesAsync();

            //var result = await dataContext.Stages.FirstOrDefaultAsync(s => s.Id == stage.Id);
            //if (result != null)
            //{
            //    _logger.LogInformation("Время: {TimeAction}. {Status} Этап: {StageId}", DateTime.Now, Status.NotDeleted, stage.Id);
            //}
        }

        public async Task DeleteProjectAsync(int userId, int projectId)
        {
            var project = new Project
            {
                Id = projectId
            };

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} Проект: {ProjectId}", DateTime.Now, 99999, Status.Deleted, project.Id);

            var dataContext = DataContext;
            dataContext.FurnitureProjects.Attach(project);
            dataContext.Entry(project).State = EntityState.Deleted;
            await dataContext.SaveChangesAsync();

            //if (dataContext.FurnitureProjects.FirstOrDefaultAsync(s => s.Id == project.Id).Result != null)
            //{
            //    _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorName}, НЕ СМОГ {Status} Проект: {ProjectName}", DateTime.Now, "Имя пользователя"/*project.Executor.Name*/, Status.Deleted, project.Id);
            //}
        }

        //TODO: Залогировать.
        public async Task SetFlagValueAsync(int userId, int projectId, int stageId, Status status)
        {
            var dataContext = DataContext;

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} в Проекте {ProjectId} Этап: {StageId}", DateTime.Now, userId, Status.Created, projectId, stageId);

            Stage stage = new Stage
            {
                Id = stageId
            };
            dataContext.Stages.Attach(stage);

            stage.Status = status;

            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<Project>> ToListProjectOrDefaultAsync(int userId)
        {
            var projects = await DataContext.FurnitureProjects
                .AsNoTracking()
                .Where(p => p.ExecutorId == userId)
                .ToListAsync();
            if (projects != null)
            {
                return projects;
            }

            return new List<Project>();
        }

        public async Task<Project> GetAllProjectDataAsync(int projectId)
        {
            var project = await DataContext.FurnitureProjects
                .AsNoTracking()
                .Include(p => p.Stages)
                .Where(p => p.Id == projectId)
                .FirstOrDefaultAsync();

            return project;
        }

        public List<Executor> GetExecutors()
        {
            var dataContext = DataContext;
            List<Executor> executors = dataContext.ExecutorUsers
                .AsNoTracking()
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.SurName
                })
                .AsEnumerable()
                .Select(un => new Executor
                {
                    Id = un.Id,
                    Name = un.Name,
                    SurName = un.SurName
                })
                .ToList();

            return executors;
        }

        //Залогировать.
        public async Task ChengeExecutorAsync(int executorId, int stageId)
        {
            Stage stage = new Stage
            {
                Id = stageId
            };

            var dataContext = DataContext;
            //Из-за того, что я создаю новый обьект и сопоставляю его с существующем,
            //а потом указываю что он изменен все поля которые не записаны считаются новыми значениями.
            dataContext.Stages.Attach(stage);
            //stage.ExecutorId = executorId;
            dataContext.Entry(stage).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }
    }
}
