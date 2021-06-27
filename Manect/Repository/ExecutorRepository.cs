using Manect.Controllers.Models;
using Manect.Data;
using Manect.Data.Entities;
using Manect.Interfaces;
using Manect.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Repository
{
    public class ExecutorRepository : IExecutorRepository
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

        public ExecutorRepository(IServiceScopeFactory serviceScopeFactory, ILogger<ExecutorRepository> logger)
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
    }
}
