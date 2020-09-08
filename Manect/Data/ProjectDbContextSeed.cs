using Manect.Data.Entities;
using Manect.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Data
{
    public class ProjectDbContextSeed
    {
        public static async Task SeedAsync(ProjectDbContext dataContext, AppIdentityDbContext identityContext)
        {

            //dataContext.FurnitureProjects.RemoveRange(dataContext.FurnitureProjects);
            //dataContext.Stages.RemoveRange(dataContext.Stages);
            //dataContext.SaveChanges();

            if (!dataContext.ExecutorUsers.Any())
            {
                await dataContext.SyncTables.UsersAsync();
            }

            if (!dataContext.FurnitureProjects.Any())
            {
                var userK = await dataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Id == 1);
                var userS = await dataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Id == 2);

                await dataContext.FurnitureProjects.AddRangeAsync(
                    GetPreconfiguredProjects(userK, userS));
               
                dataContext.SaveChanges();
            }
        }

        static IEnumerable<Project> GetPreconfiguredProjects(ExecutorUser userK, ExecutorUser userS)
        {
            return new List<Project>()
            {
                new Project("Кухня", new DateTime(2020, 9, 24), 160000, userK,
                        new List<Stage>()
                        {
                            new Stage("Обсуждение пожеланий клиента, предварительный эскиз ", new DateTime(2020, 8, 25),userK, comment: "Обосрался по жесткой."),
                            new Stage(" Замер помещения", new DateTime(2020, 8, 25),userS,comment: "Срочно!"),
                            new Stage("Окончательный эскиз", new DateTime(2020, 8, 27), userK, comment: "Что то не получается, спрошу у Кости."),
                            new Stage("Просчёт", new DateTime(2020, 8, 28), userK),
                            new Stage("Дополнительные комплектующие и нюансы", new DateTime(2020, 8, 30), userK),
                            new Stage("Производство", new DateTime(2020, 9, 28), userK),
                            new Stage("Монтаж", new DateTime(2020, 9, 30),userS),
                            new Stage("Сдача объекта", new DateTime(2020, 9, 30), userK)
                        }),

                new Project("Туалет", new DateTime(2020, 10, 22), 260000, userK,
                        new List<Stage>()
                        {
                            new Stage("Производство", new DateTime(2020, 9, 28), userK)
                        }),


                new Project("Шкаф", new DateTime(2020, 11, 11), 50000, userK,
                        new List<Stage>()
                        {
                            new Stage("Производство", new DateTime(2020, 9, 28), userK)
                        }),
                new Project("Спальня", new DateTime(2020, 11, 11), 50000, userS,
                        new List<Stage>()
                        {
                            new Stage("Производство", new DateTime(2020, 9, 28), userS)
                        })
            };
        }
    }
}
