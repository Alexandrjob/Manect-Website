using Manect.Data;
using Manect.Data.Entities;
using Manect.Identity;
using Manect.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Services
{
    public class SyncTables : ISyncTables
    {
        public static ProjectDbContext DataContext { get; set; }
        public static AppIdentityDbContext IdentityContext { get; set; }

        public SyncTables(ProjectDbContext dataContext, AppIdentityDbContext identityContext)
        {
            DataContext = dataContext;
            IdentityContext = identityContext;
        }

        public async Task UsersAsync()
        {
            var users = await IdentityContext.Users
                .Select(c => new
                {
                    c.UserName,
                    c.Email
                })
                .ToListAsync();

            await AddUsersAsync(users);
        }

        public void AddEventHandler()
        {
            IdentityContext.Users.Local
        .CollectionChanged += async (sender, args) =>
        {
            if (args.NewItems != null)
            {
                await AddUsersAsync(args.NewItems);
            }
            //TODO: Реализовать алгоритм, про котором, когда удаляется пользователь, удалялось и все связанное с ним.
            if (args.OldItems != null)
            {
                foreach (ApplicationUser c in args.OldItems)
                {

                }
            }
        };
        }

        private async Task AddUsersAsync(IList users)
        {
            var newUsers = new List<ExecutorUser>();

            foreach (ApplicationUser user in users)
            {
                var newUser = new ExecutorUser()
                {
                    Name = user.UserName,
                    Email = user.Email,
                };

                newUsers.Add(newUser);
            }
            await DataContext.ExecutorUsers.AddRangeAsync(newUsers);
            await DataContext.SaveChangesAsync();
        }
    }
}