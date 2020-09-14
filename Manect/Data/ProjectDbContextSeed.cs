using Manect.Data.Entities;
using Manect.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Data
{
    public class ProjectDbContextSeed
    {
        public static async Task SeedAsync(ProjectDbContext dataContext, ISyncTables syncTables)
        {

            //dataContext.FurnitureProjects.RemoveRange(dataContext.FurnitureProjects);
            //dataContext.Stages.RemoveRange(dataContext.Stages);
            //dataContext.SaveChanges();

            if (!dataContext.ExecutorUsers.Any())
            {
                await syncTables.UsersAsync();
            }

            if (!dataContext.FurnitureProjects.Any())
            {
                var userK = await dataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Name == "Kostya");
                var userS = await dataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Name == "Sasha");
                if (userK != null & userS != null)
                {
                    await dataContext.FurnitureProjects.AddRangeAsync(
                        GetPreconfiguredProjects(userK, userS));

                    dataContext.SaveChanges();
                }
            }
        }

        static IEnumerable<Project> GetPreconfiguredProjects(ExecutorUser userK, ExecutorUser userS)
        {
            return new List<Project>()
            {
                new Project("Кухня", 160000, userK,
                        new List<Stage>()
                        {
                            new Stage("Обсуждение пожеланий клиента, предварительный эскиз ",userK, comment: "Обосрался по жесткой."),
                            new Stage(" Замер помещения",userS,comment: "Срочно!"),
                            new Stage("Окончательный эскиз", userK, comment: "Что то не получается, спрошу у Кости."),
                            new Stage("Просчёт", userK),
                            new Stage("Дополнительные комплектующие и нюансы", userK),
                            new Stage("Производство", userK),
                            new Stage("Монтаж",userS),
                            new Stage("Сдача объекта", userK)
                        }),

                new Project("Туалет", 260000, userK,
                        new List<Stage>()
                        {
                            new Stage("макет", userK)
                        }),


                new Project("Шкаф", 50000, userK,
                        new List<Stage>()
                        {
                            new Stage("Встреча с клиентом", userK)
                        }),
                new Project("Спальня", 50000, userS,
                        new List<Stage>()
                        {
                            new Stage("Редактирование договора", userS)
                        })
            };
        }
    }
}
