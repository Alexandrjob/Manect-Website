using DomaMebelSite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomaMebelSite.Data
{
    public class ProjectDbContextSeed
    {
        public static async System.Threading.Tasks.Task SeedAsync(ProjectDbContext context)
        {
            if (!context.FurnitureProjects.Any())
            {
                await context.FurnitureProjects.AddRangeAsync(
                    GetPreconfiguredProjects());

                context.SaveChanges();
            }
        }

        static IEnumerable<Project> GetPreconfiguredProjects()
        {
            return new List<Project>()
            {
                new Project("Кухня", new DateTime(2020, 9, 24), 160000,
                        new List<Stage>()
                        {
                            new Stage("Обсуждение пожеланий клиента, предварительный эскиз ", new DateTime(2020, 8, 25),comment: "Обосрался по жесткой."),
                            new Stage(" Замер помещения", new DateTime(2020, 8, 25),"Костя",comment: "Срочно!"),
                            new Stage("Окончательный эскиз", new DateTime(2020, 8, 27),comment: "Что то не получается, спрошу у Кости."),
                            new Stage("Просчёт", new DateTime(2020, 8, 28)),
                            new Stage("Дополнительные комплектующие и нюансы", new DateTime(2020, 8, 30)),
                            new Stage("Производство", new DateTime(2020, 9, 28)),
                            new Stage("Монтаж", new DateTime(2020, 9, 30),"Костя"),
                            new Stage("Сдача объекта", new DateTime(2020, 9, 30))
                        })
            };
        }
    }
}
