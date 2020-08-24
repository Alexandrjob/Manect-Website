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
                            new Stage("Создание чертежа", new DateTime(2020, 8, 25),comment: "Обосрался по жесткой."),
                            new Stage("Взятие замеров", new DateTime(2020, 8, 27),comment: "Срочно!"),
                            new Stage("Создание готового чертежа", new DateTime(2020, 8, 30),comment: "Что то не получается, спрошу у Кости."),
                            new Stage("Подписание документов", new DateTime(2020, 9, 2), "Костя")
                        })
            };
        }
    }
}
