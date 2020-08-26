using Manect.Entities;
using Manect.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Data
{
    public class ProjectDbContextSeed
    {
        public static async Task SeedAsync(ProjectDbContext context, UserManager<ApplicationUser> userManager)
        {
            //context.FurnitureProjects.RemoveRange(context.FurnitureProjects);
            //context.Stages.RemoveRange(context.Stages);

            //context.SaveChanges();

            if (!context.FurnitureProjects.Any())
            {
                var user = await userManager.FindByEmailAsync("Sasha@gmail.com");
                var userK = await userManager.FindByEmailAsync("Kostya@gmail.com");

                await context.FurnitureProjects.AddRangeAsync(
                    GetPreconfiguredProjectsAsync(user, userK));

                context.SaveChanges();
            }
        }

        static IEnumerable<Project> GetPreconfiguredProjectsAsync(ApplicationUser user, ApplicationUser userK)
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
