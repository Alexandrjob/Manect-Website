using Manect.Data.Entities;
using Manect.Identity;
using Manect.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
                var syncTables = new SyncTables(dataContext, identityContext);
                await syncTables.UsersAsync();
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
