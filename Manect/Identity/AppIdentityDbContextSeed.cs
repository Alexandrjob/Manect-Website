using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System.Threading.Tasks;

namespace Manect.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var SashaUser = new ApplicationUser 
                { 
                    UserName = "Sasha",
                    FirstName = "Саша",
                    LastName = "Поползин",
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
    }
}
