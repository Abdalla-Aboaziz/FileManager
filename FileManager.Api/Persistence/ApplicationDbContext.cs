using FileManager.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FileManager.Api.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):DbContext(options)
    {

        public DbSet<UploadedFiles> Files { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}
