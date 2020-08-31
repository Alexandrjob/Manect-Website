using Manect.Data.Entities;
using Manect.Entities;
using Manect.Identity;
using Manect.Services;
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
            //dataContext.Stages.RemoveRange(dataContext.Stages);
            //dataContext.FurnitureProjects.RemoveRange(dataContext.FurnitureProjects);
            //dataContext.SaveChanges();

            if (!dataContext.ExecutorUsers.Any())
            {
                await SyncTables.UsersAsync(dataContext, identityContext);
            }

            if (!dataContext.FurnitureProjects.Any())
            {
                //TODO: Оптимизировать (1 и 2 убрать) сделать автоматически
                var user1 = await dataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Id == 1);
                var user2 = await dataContext.ExecutorUsers.FirstOrDefaultAsync(user => user.Id == 2);

                await dataContext.FurnitureProjects.AddRangeAsync(
                    GetPreconfiguredProjectsAsync(user1, user2));

                dataContext.SaveChanges();
            }
        }

        static IEnumerable<Project> GetPreconfiguredProjectsAsync(ExecutorUser user, ExecutorUser userK)
        {
            return new List<Project>()
            {
                new Project("Кухня", new DateTime(2020, 9, 24), 160000, user,
                        new List<Stage>()
                        {
                            new Stage("Обсуждение пожеланий клиента, предварительный эскиз ", new DateTime(2020, 8, 25),comment: "Обосрался по жесткой."),
                            new Stage(" Замер помещения", new DateTime(2020, 8, 25),userK,comment: "Срочно!"),
                            new Stage("Окончательный эскиз", new DateTime(2020, 8, 27),comment: "Что то не получается, спрошу у Кости."),
                            new Stage("Просчёт", new DateTime(2020, 8, 28)),
                            new Stage("Дополнительные комплектующие и нюансы", new DateTime(2020, 8, 30)),
                            new Stage("Производство", new DateTime(2020, 9, 28)),
                            new Stage("Монтаж", new DateTime(2020, 9, 30),userK),
                            new Stage("Сдача объекта", new DateTime(2020, 9, 30))
                        })
            };
        }
    }
}
