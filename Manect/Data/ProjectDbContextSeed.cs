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
                var user2 = executors.FirstOrDefault(u => u.UserName == "Sasha");
                if (executors != default)
                {
                    for(int i = 0; i < executors.Count; i++)
                    {
                        await dataContext.Projects.AddRangeAsync(
                        GetPreconfiguredProjects(executors[i], user2));
                    }

                    dataContext.SaveChanges();
                }
            }
        }

        static IEnumerable<Project> GetPreconfiguredProjects(Executor user1, Executor user2)
        {
            return new List<Project>()
            {
                new Project("Кухня", 160000, user1,
                        new List<Stage>()
                        {
                            new Stage("Обсуждение пожеланий клиента, предварительный эскиз ",user1),
                            new Stage(" Замер помещения", user2,comment: "Срочно!"),
                            new Stage("Окончательный эскиз", user1, comment: "Что то не получается, спрошу у Кости."),
                            new Stage("Просчёт", user1),
                            new Stage("Дополнительные комплектующие и нюансы", user1),
                            new Stage("Производство", user1),
                            new Stage("Монтаж",user2),
                            new Stage("Сдача объекта", user1)
                        }),

                new Project("Туалет", 260000, user1,
                        new List<Stage>()
                        {
                            new Stage("макет", user2)
                        }),


                new Project("Шкаф", 50000, user1,
                        new List<Stage>()
                        {
                            new Stage("Встреча с клиентом", user1)
                        }),
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
