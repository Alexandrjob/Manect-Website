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
            if (!dataContext.Executors.Any())
            {
                await syncTables.UsersAsync();
            }

            if (!dataContext.FurnitureProjects.Any())
            {
                var userKostya = await dataContext.Executors.Where(user => user.Name == "Kostya").FirstOrDefaultAsync();
                var userSasha = await dataContext.Executors.Where(user => user.Name == "Sasha").FirstOrDefaultAsync();
                if (userKostya != default & userSasha != default)
                {
                    await dataContext.FurnitureProjects.AddRangeAsync(
                        GetPreconfiguredProjects(userKostya, userSasha));

                    dataContext.SaveChanges();
                }
            }
        }

        static IEnumerable<Project> GetPreconfiguredProjects(Executor userKostya, Executor userSasha)
        {
            return new List<Project>()
            {
                new Project("Кухня", 160000, userKostya,
                        new List<Stage>()
                        {
                            new Stage("Обсуждение пожеланий клиента, предварительный эскиз ",userKostya, comment: "Обосрался по жесткой."),
                            new Stage(" Замер помещения", userSasha,comment: "Срочно!"),
                            new Stage("Окончательный эскиз", userKostya, comment: "Что то не получается, спрошу у Кости."),
                            new Stage("Просчёт", userKostya),
                            new Stage("Дополнительные комплектующие и нюансы", userKostya),
                            new Stage("Производство", userKostya),
                            new Stage("Монтаж",userSasha),
                            new Stage("Сдача объекта", userKostya)
                        }),

                new Project("Туалет", 260000, userKostya,
                        new List<Stage>()
                        {
                            new Stage("макет", userKostya)
                        }),


                new Project("Шкаф", 50000, userKostya,
                        new List<Stage>()
                        {
                            new Stage("Встреча с клиентом", userKostya)
                        }),
                new Project("Спальня", 50000, userSasha,
                        new List<Stage>()
                        {
                            new Stage("Редактирование договора", userSasha)
                        })
            };
        }
    }
}
