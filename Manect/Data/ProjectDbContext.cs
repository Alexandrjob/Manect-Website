using Manect.Data.Entities;
using Manect.DataBaseLogger;
using Microsoft.EntityFrameworkCore;

namespace Manect.Data
{
    public class ProjectDbContext: DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {

        }

        public DbSet<Stage> Stages { get; set; }
        public DbSet<Project> FurnitureProjects { get; set; }
        public DbSet<Executor> ExecutorUsers { get; set; }
        public DbSet<LogItem> LogUserActivity { get; set; }
    }
}
