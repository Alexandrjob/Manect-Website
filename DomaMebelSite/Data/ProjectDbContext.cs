using Microsoft.EntityFrameworkCore;
using DomaMebelSite.Entities;

namespace DomaMebelSite.Data
{
    public class ProjectDbContext: DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }

        public DbSet<Stage> Stages { get; set; }
        public DbSet<Project> FurnitureProjects { get; set; }
    }
}
