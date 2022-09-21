using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manect.Data;
using Manect.Data.Entities;
using Manect.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manect.Repository
{
    public class ProjectRepository : IProjectRepository
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

        public ProjectRepository(IServiceScopeFactory serviceScopeFactory, ILogger<ProjectRepository> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
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
            }

            await dataContext.SaveChangesAsync();

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} Проект {ProjectId}",
                DateTime.Now, user.Id, Status.Created, project.Id);
        }

        public async Task DeleteProjectAsync(int userId, int projectId)
        {
            var dataContext = DataContext;
            var project = await dataContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            project.Status = Status.Deleted;
            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} Проект: {ProjectId}",
                DateTime.Now, userId, Status.Deleted, project.Id);

            dataContext.Entry(project).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<Project>> GetProjectOrDefaultToListAsync(int userId)
        {
            var projects = await DataContext.Projects
                .AsNoTracking()
                .Where(p => p.Executor.Id == userId && p.Status != Status.Deleted)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.CreationDate
                })
                .AsQueryable()
                .Select(pr => new Project
                {
                    Id = pr.Id,
                    Name = pr.Name,
                    CreationDate = pr.CreationDate
                })
                .ToListAsync();

            if (projects != null)
            {
                return projects;
            }

            return new List<Project>();
        }

        public async Task<Project> GetProjectAllDataAsync(int projectId)
        {
            var project = await DataContext.Projects
                .AsNoTracking()
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.ExecutorId,
                    Stages = p.Stages.Where(s => s.Status != Status.Deleted).Select(s => new
                    {
                        s.Id,
                        s.Name,
                        s.Status,
                        s.ExpirationDate,
                        s.CreationDate,
                        s.Comment,
                        s.ExecutorId,
                        Executor = new
                        {
                            s.Executor.Id,
                            s.Executor.FirstName,
                            s.Executor.LastName
                        }
                    }).ToList(),
                })
                .AsQueryable()
                .Select(project => new Project
                {
                    Id = project.Id,
                    Name = project.Name,
                    ExecutorId = project.ExecutorId,
                    Stages = project.Stages.Select(stage => new Stage
                    {
                        Id = stage.Id,
                        Name = stage.Name,
                        Status = stage.Status,
                        ExpirationDate = stage.ExpirationDate,
                        CreationDate = stage.CreationDate,
                        Comment = stage.Comment,
                        ExecutorId = stage.ExecutorId,
                        Executor = new Executor()
                        {
                            Id = stage.Executor.Id,
                            FirstName = stage.Executor.FirstName,
                            LastName = stage.Executor.LastName
                        }
                    }).ToList()
                }).FirstOrDefaultAsync(p => p.Id == projectId);

            return project;
        }

        public async Task<Project> GetProjectInfoAsync(int projectId)
        {
            var project = await DataContext.Projects
                .AsNoTracking()
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.CreationDate,
                    p.ExpirationDate,
                    p.Price
                })
                .AsQueryable()
                .Select(project => new Project
                {
                    Id = project.Id,
                    Name = project.Name,
                    CreationDate = project.CreationDate,
                    ExpirationDate = project.ExpirationDate,
                    Price = project.Price
                }).FirstOrDefaultAsync(p => p.Id == projectId);

            return project;
        }

        public async Task EditProjectAsync(Project project, int CurrentUserId)
        {
            var dataContext = DataContext;

            project.ExecutorId = await dataContext.Projects.Where(p => p.Id == project.Id).Select(p => p.ExecutorId)
                .FirstOrDefaultAsync();

            _logger.LogInformation("Время: {TimeAction}. Пользователь {ExecutorId}, {Status} Проект {ProjectId}",
                DateTime.Now, CurrentUserId, Status.Modified, project.Id);

            dataContext.Projects.Attach(project);
            dataContext.Entry(project).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
        }
    }
}