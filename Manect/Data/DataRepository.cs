using Manect.Data.Entities;
using Manect.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public DataRepository(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        public async Task<ExecutorUser> FindUserByNameOrDefaultAsync(string name)
        {;
            using var scope = _serviceScopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetService<ProjectDbContext>();

            return await dataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Name == name);
        }

        public async Task<ExecutorUser> FindUserByEmailAsync(string email)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetService<ProjectDbContext>();

            return await dataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task AddStageAsync(ExecutorUser user, Project project)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetService<ProjectDbContext>();

            //TODO: может получше можно придумать?
            var stage = new Stage("Тест2", user)
            {
                ProjectId = project.Id
            };
            
            dataContext.Entry(stage).State = EntityState.Added;
            await dataContext.SaveChangesAsync();

        }

        //TODO: сделать метод универсальным (получать значение по которому можно понять какой проект(шаблоны будут записаны в новой таблице) нужно создать)
        public async Task AddProjectDefaultAsync(ExecutorUser user)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetService<ProjectDbContext>();

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
            dataContext.Entry(project).State = EntityState.Added;
            await dataContext.SaveChangesAsync();
        }

        public async Task<List<Project>> ToListProjectsAsync(ExecutorUser user)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetService<ProjectDbContext>();
            var projects = await dataContext.FurnitureProjects.Where(p => p.Executor == user).ToListAsync();
            if (projects != null)
            {
                return projects;
            }

            return new List<Project>();
        }
    }
}
