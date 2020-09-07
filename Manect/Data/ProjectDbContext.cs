using Microsoft.EntityFrameworkCore;
using Manect.Data.Entities;
using Manect.Interfaces;
using Manect.Services;
using Manect.Identity;

namespace Manect.Data
{
    public class ProjectDbContext: DbContext
    {
        public readonly ISyncTables SyncTables;

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options, AppIdentityDbContext identityContext/*, ISyncTables syncTables*/) : base(options)
        {
            //TODO: В будущем изметь(наверное).
            SyncTables = new SyncTables(this, identityContext);
            SyncTables.AddEventHandler();
        }

        public DbSet<Stage> Stages { get; set; }
        public DbSet<Project> FurnitureProjects { get; set; }
        public DbSet<ExecutorUser> ExecutorUsers { get; set; }
    }
}
