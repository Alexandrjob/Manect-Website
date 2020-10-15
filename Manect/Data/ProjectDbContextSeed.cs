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
            if (!dataContext.ExecutorUsers.Any())
            {
                await syncTables.UsersAsync();
            }

            if (!dataContext.FurnitureProjects.Any())
            {
                var userKostyaId = await dataContext.ExecutorUsers.Where(user => user.Name == "Kostya").Select(u => u.Id).FirstOrDefaultAsync();
                var userSashaId = await dataContext.ExecutorUsers.Where(user => user.Name == "Sasha").Select(u=>u.Id).FirstOrDefaultAsync();
                if (userKostyaId != default & userSashaId != default)
                {
                    await dataContext.FurnitureProjects.AddRangeAsync(
                        GetPreconfiguredProjects(userKostyaId, userSashaId));

                    dataContext.SaveChanges();
                }
            }
        }

        static IEnumerable<Project> GetPreconfiguredProjects(int userKostyaId, int userSashaId)
        {
            return new List<Project>()
            {
                new Project("Кухня", 160000, userKostyaId,
                        new List<Stage>()
                        {
                            new Stage("Обсуждение пожеланий клиента, предварительный эскиз ",userKostyaId, comment: "Обосрался по жесткой."),
                            new Stage(" Замер помещения", userSashaId,comment: "Срочно!"),
                            new Stage("Окончательный эскиз", userKostyaId, comment: "Что то не получается, спрошу у Кости."),
                            new Stage("Просчёт", userKostyaId),
                            new Stage("Дополнительные комплектующие и нюансы", userKostyaId),
                            new Stage("Производство", userKostyaId),
                            new Stage("Монтаж",userSashaId),
                            new Stage("Сдача объекта", userKostyaId)
                        }),

                new Project("Туалет", 260000, userKostyaId,
                        new List<Stage>()
                        {
                            new Stage("макет", userKostyaId)
                        }),


                new Project("Шкаф", 50000, userKostyaId,
                        new List<Stage>()
                        {
                            new Stage("Встреча с клиентом", userKostyaId)
                        }),
                new Project("Спальня", 50000, userSashaId,
                        new List<Stage>()
                        {
                            new Stage("Редактирование договора", userSashaId)
                        })
            };
        }
    }
}
