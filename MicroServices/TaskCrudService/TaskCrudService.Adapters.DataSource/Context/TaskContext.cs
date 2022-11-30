using Microsoft.EntityFrameworkCore;
using TaskCrudService.Domain.Entities;

namespace TaskCrudService.Adapters.DataSource.Context
{
    public class TaskContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; } = null!;
        public DbSet<TypeEntity> Types { get; set; } = null!;
        public DbSet<ArgumentEntity> Arguments { get; set; } = null!;
        public DbSet<ArgumentTypeEntity> ArgumentTypes { get; set; } = null!;

        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<TypeEntity>()
            //            .HasIndex(us => us.Name)
            //            .IsUnique();

            //modelBuilder.Entity<ArgumentTypeEntity>()
            //            .HasIndex(us => us.Name)
            //            .IsUnique();
        }

        
    }
}
