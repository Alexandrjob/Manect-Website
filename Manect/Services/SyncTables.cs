using Manect.Data;
using Manect.Data.Entities;
using Manect.Identity;
using Manect.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            var users = await IdentityContext.Users.
                OrderBy(c => c.Email).
                Select(c => new
                {
                    c.UserName,
                    c.Email
                }).
                ToListAsync();

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
            DataContext.ExecutorUsers.AddRange(newUsers);
            DataContext.SaveChanges();
        }

        public void AddEventHandler()
        {
            //TODO: Разделить на методы
            IdentityContext.Users.Local
        .CollectionChanged += async (sender, args) =>
        {
            if (args.NewItems != null)
            {
                var newUsers = new List<ExecutorUser>();

                foreach (ApplicationUser c in args.NewItems)
                {
                    var newUser = new ExecutorUser()
                    {
                        Name = c.UserName,
                        Email = c.Email,
                    };

                    newUsers.Add(newUser);  
                }
                await DataContext.ExecutorUsers.AddRangeAsync(newUsers);
                await DataContext.SaveChangesAsync();
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
    }
}