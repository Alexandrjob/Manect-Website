using Manect.Data;
using Manect.Data.Entities;
using Manect.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manect.Services
{
    public class SyncTables
    {
        public static async Task UsersAsync(ProjectDbContext dataContext, AppIdentityDbContext identityContext)
        {
            //TODO: настроить на то чтобы вытаскивал только имя и емэил.
            var users = await identityContext.Users.ToListAsync();

            var newUsers = new List<ExecutorUser>();

            foreach (var user in users)
            {
                var newUser = new ExecutorUser()
                {
                    Name = user.UserName,
                    Email = user.Email,
                };

                newUsers.Add(newUser);
            }
            dataContext.ExecutorUsers.AddRange(newUsers);
            dataContext.SaveChanges();
        }

    }
}