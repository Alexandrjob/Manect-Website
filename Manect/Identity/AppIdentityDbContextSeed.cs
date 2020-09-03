using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;

namespace Manect.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //TODO: узнать в чем ошибка.
            if (!userManager.Users.Any())
            {
                var SashatUser = new ApplicationUser { UserName = "Sasha", Email = "Sasha@gmail.com" };
                var KostyatUser = new ApplicationUser { UserName = "Kostya", Email = "Kostya@gmail.com" };

                await userManager.CreateAsync(SashatUser, "325DR_qwer");
                await userManager.CreateAsync(KostyatUser, "325DR_qwer");
            }

        }
    }
}
