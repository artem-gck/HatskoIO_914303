using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.DataAccess.dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public RoleDto Role { get; set; }
        public TokensDto? Tokens { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
