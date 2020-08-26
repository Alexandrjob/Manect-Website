using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Manect.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var SashatUser = new ApplicationUser { UserName = "Sasha", Email = "Sasha@gmail.com" };
            var KostyatUser = new ApplicationUser { UserName = "Kostya", Email = "Kostya@gmail.com" };

            await userManager.CreateAsync(SashatUser, "325DR_qwer");
            await userManager.CreateAsync(KostyatUser, "325DR_qwer");
        }
    }
}
