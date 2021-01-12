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
                var SashaUser = new ApplicationUser
                {
                    UserName = "Sasha",
                    FirstName = "Саша",
                    LastName = "Сваровский",
                    Email = "Sasha@gmail.com"
                };

                var KostyaUser = new ApplicationUser
                {
                    UserName = "Kostya",
                    FirstName = "Костя",
                    LastName = "Кузнецов",
                    Email = "Kostya@gmail.com"
                };

                var KatyaUser = new ApplicationUser
                {
                    UserName = "Katya",
                    FirstName = "Катя",
                    LastName = "Кузнецова",
                    Email = "Katya@gmail.com"
                };

                var AnyaUser = new ApplicationUser
                {
                    UserName = "Anya",
                    FirstName = "Аня",
                    LastName = "Анова",
                    Email = "Anya@gmail.com"
                };

                var LizaUser = new ApplicationUser
                {

                    UserName = "Liza",
                    FirstName = "Лиза",
                    LastName = "Лизова",
                    Email = "Liza@gmail.com"
                };

                await userManager.CreateAsync(SashaUser, "325DR_qwer");
                await userManager.CreateAsync(KostyaUser, "325DR_qwer");
                await userManager.CreateAsync(KatyaUser, "325DR_qwer");
                await userManager.CreateAsync(AnyaUser, "325DR_qwer");
                await userManager.CreateAsync(LizaUser, "325DR_qwer");
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
