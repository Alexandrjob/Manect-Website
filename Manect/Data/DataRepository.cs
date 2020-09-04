using Manect.Data.Entities;
using Manect.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Data
{
    public class DataRepository : IDataRepository 
    {
        public ProjectDbContext DataContext { get; private set; }

        public DataRepository(ProjectDbContext dataContext)
        {
            DataContext = dataContext;
        }

        public Task<ExecutorUser> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> ToListProjectsAsync(string userName)
        {
            //TODO: Сделать проверку, были ли загружены ранее данные о заказах,
            // для этого пользователя, если нет, то использовать явную загрузку.
            //if (DataContext.Entry(user).Collection(p => p.Projects).IsLoaded)
            //{
            //    DataContext.ExecutorUsers.Include(p => p.Projects).Load();
            //}
            DataContext.FurnitureProjects.Include(p => p.Executor).Load();

            if (DataContext.FurnitureProjects.Any(p => p.Executor.Name == userName))
            {
                return await DataContext.FurnitureProjects
                .AsNoTracking()
                .Where(p => p.Executor.Name == userName)
                .ToListAsync();
            }
            return null;
        }
    }
}
