using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.Services.dto.context
{
    public class UsersContext : DbContext
    {
        public DbSet<UserDto> Users { get; set; } = null!;
        public DbSet<UserInfoDto> UsersInfo { get; set; } = null!;
        public DbSet<RoleDto> Roles { get; set; } = null!;
        public DbSet<TokensDto> Tokens { get; set; } = null!;

        public UsersContext(DbContextOptions<UsersContext> options) 
            : base(options)
        {

        }
    }
}
