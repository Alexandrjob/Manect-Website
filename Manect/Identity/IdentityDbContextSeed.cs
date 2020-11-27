using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Identity
{
    public class IdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IdentityDbContext identityDbContext)
        {
            //ClearIdentityBase(identityDbContext);

            await identityDbContext.SaveChangesAsync();

            if (!userManager.Users.Any())
            {
                //Тут Должны быть ребята, но их почему-то нет

            }
        }

        private static void ClearIdentityBase(IdentityDbContext identityDbContext)
        {
            identityDbContext.Users.RemoveRange(identityDbContext.Users);
            identityDbContext.RoleClaims.RemoveRange(identityDbContext.RoleClaims);
            identityDbContext.Roles.RemoveRange(identityDbContext.Roles);
            identityDbContext.UserClaims.RemoveRange(identityDbContext.UserClaims);
            identityDbContext.UserRoles.RemoveRange(identityDbContext.UserRoles);
            identityDbContext.UserTokens.RemoveRange(identityDbContext.UserTokens);
            identityDbContext.UserLogins.RemoveRange(identityDbContext.UserLogins);
        }
    }
}
