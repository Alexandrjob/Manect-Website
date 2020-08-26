using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DomaMebelSite.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new IdentityUser { UserName = "Sasha", Email = "Sasha@gmail.com" };
            await userManager.CreateAsync(defaultUser, "325DR_qwer");
        }
    }
}
