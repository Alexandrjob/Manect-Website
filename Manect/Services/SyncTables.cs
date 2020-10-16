using Manect.Data;
using Manect.Data.Entities;
using Manect.Identity;
using Manect.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manect.Services
{
    public class SyncTables : ISyncTables
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private ProjectDbContext DataContext
        {
            get
            {
                var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetService<ProjectDbContext>();
            }
        }

        private AppIdentityDbContext IdentityContext
        {
            get
            {
                var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetService<AppIdentityDbContext>();
            }
        }

        public SyncTables(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task UsersAsync()
        {
            List<Executor> identityUsers = IdentityContext.Users
                .Select(c => new
                {
                    c.UserName,
                    c.FirstName,
                    c.LastName,
                    c.Email
                })
                .AsEnumerable()
                .Select(an => new Executor
                {
                    UserName = an.UserName,
                    FirstName = an.FirstName,
                    LastName = an.LastName,
                    Email = an.Email
                })
                .ToList();

            var dataContext = DataContext;
            await dataContext.Executors.AddRangeAsync(identityUsers);
            await dataContext.SaveChangesAsync();
        }

        //public void AddEventHandler()
        //{
        //    IdentityContext.Users.Local
        //        .CollectionChanged += async (sender, args) =>
        //        {
        //            if (args.NewItems != null)
        //            {
        //                await AddUsersAsync(args.NewItems);
        //            }
        //            //TODO: В будущем реализовать алгоритм, про котором, когда удаляется пользователь, удалялось и все связанное с ним.
        //            if (args.OldItems != null)
        //            {
        //                foreach (ApplicationUser c in args.OldItems)
        //                {

        //                }
        //            }
        //        };
        //}

        //private async Task AddUsersAsync(IList users)
        //{
        //    var newUsers = new List<ExecutorUser>();

        //    foreach (ApplicationUser user in users)
        //    {
        //        var newUser = new ExecutorUser()
        //        {
        //            Name = user.UserName,
        //            Email = user.Email,
        //        };

        //        newUsers.Add(newUser);
        //    }

        //    var dataContext = DataContext;
        //    await dataContext.ExecutorUsers.AddRangeAsync(newUsers);
        //    await dataContext.SaveChangesAsync();
        //}
    }
}