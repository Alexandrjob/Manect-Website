using Manect.Data.Entities;
using Manect.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Data
{
    public class ProjectDbContextSeed
    {
        public static async Task SeedAsync(ProjectDbContext dataContext, ISyncTables syncTables)
        {
            //await ClearDataBase(dataContext);

            if (!dataContext.Executors.Any())
            {
                await syncTables.UsersAsync();
            }

            if (!dataContext.Projects.Any())
            {
                var executors = await dataContext.Executors.ToListAsync();
                if (executors != default)
                {
                    for (int i = 0; i < executors.Count; i++)
                    {
                        await dataContext.Projects.AddRangeAsync(
                        GetPreconfiguredProjects(executors[i]));
                    }

                    dataContext.SaveChanges();
                }
            }
        }

        static IEnumerable<Project> GetPreconfiguredProjects(Executor user1)
        {
            return new List<Project>()
            {
                new Project("Кухня", 160000, user1, GetStageList(user1)),

                new Project("Туалет", 260000, user1, GetStageList(user1)),

                new Project("Шкаф", 50000, user1, GetStageList(user1)),
            };
        }

        private static List<Stage> GetStageList(Executor user1)
        {
            return new List<Stage>()
                        {
                            new Stage("Встреча  с клиентом", user1),
                            new Stage("Замер обьекта", user1),
                            new Stage("Просчет", user1),
                            new Stage("Эскиз", user1),
                            new Stage("Материалы и счета", user1),
                            new Stage("Монтаж", user1),
                            new Stage("Нюансы проекта", user1)
                        };
        }

        static async Task ClearDataBase(ProjectDbContext dataContext)
        {
            dataContext.Executors.RemoveRange(dataContext.Executors);
            dataContext.Projects.RemoveRange(dataContext.Projects);
            dataContext.Stages.RemoveRange(dataContext.Stages);
            await dataContext.SaveChangesAsync();
        }
    }
}
