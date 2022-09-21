using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manect.Data;
using Manect.Data.Entities;
using Manect.Identity;
using Manect.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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

        private IdentityDbContext IdentityContext
        {
            get
            {
                var scope = _serviceScopeFactory.CreateScope();
                return scope.ServiceProvider.GetService<IdentityDbContext>();
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
    }
}