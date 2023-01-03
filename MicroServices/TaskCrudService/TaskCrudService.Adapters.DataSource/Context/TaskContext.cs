using Microsoft.EntityFrameworkCore;
using TaskCrudService.Domain.Entities;

namespace TaskCrudService.Adapters.DataSource.Context
{
    public class TaskContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; } = null!;
        public DbSet<TypeEntity> Types { get; set; } = null!;
        public DbSet<PerformerEntity> Performers { get; set; } = null!;
        public DbSet<DocumentEntity> Documents { get; set; } = null!;

        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
            Database.Migrate();
        }
    }
}
