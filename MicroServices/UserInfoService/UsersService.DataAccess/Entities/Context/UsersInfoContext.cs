using Microsoft.EntityFrameworkCore;

namespace UsersService.DataAccess.Entities.Context
{
    /// <summary>
    /// Context for users info.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class UsersInfoContext : DbContext
    {
        public DbSet<UserInfoEntity> UsersInfo { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersInfoContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public UsersInfoContext(DbContextOptions<UsersInfoContext> options) 
            : base(options)
        {

        }
    }
}
