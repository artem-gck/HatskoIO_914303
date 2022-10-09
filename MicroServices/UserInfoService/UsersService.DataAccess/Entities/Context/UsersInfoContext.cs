using Microsoft.EntityFrameworkCore;

namespace UsersService.DataAccess.Entities.Context
{
    public class UsersInfoContext : DbContext
    {
        public DbSet<UserEntity> UsersInfo { get; set; } = null!;

        public UsersInfoContext(DbContextOptions<UsersInfoContext> options) 
            : base(options)
        {

        }
    }
}
