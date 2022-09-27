using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.DataAccess.Entities.Context
{
    public class UsersInfoContext : DbContext
    {
        public DbSet<UserInfoEntity> UsersInfo { get; set; } = null!;
        public DbSet<PositionEntity> Positions { get; set; } = null!;

        public UsersInfoContext(DbContextOptions<UsersInfoContext> options) 
            : base(options)
        {

        }
    }
}
