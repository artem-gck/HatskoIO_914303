using Microsoft.EntityFrameworkCore;

namespace UsersService.DataAccess.Entities.Context
{
    public class UsersContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; } = null!;

        public UsersContext(DbContextOptions<UsersContext> options) 
            : base(options)
        {
            Database.Migrate();
        }
    }
}
