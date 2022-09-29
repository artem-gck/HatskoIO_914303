using Microsoft.EntityFrameworkCore;
using UserLoginService.Domain.Entities;

namespace UserLoginService.Repository.Context
{
    public class UserLoginContext : DbContext
    {
        public DbSet<UserLoginEntity> UserLogin { get; set; } = null!;
        public DbSet<RoleEntity> Role { get; set; } = null!;

        public UserLoginContext(DbContextOptions<UserLoginContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLoginEntity>()
                .HasIndex(u => u.TockenId)
                .IsUnique();

            modelBuilder.Entity<UserLoginEntity>()
                .HasIndex(u => u.UserInfoId)
                .IsUnique();
        }
    }
}
