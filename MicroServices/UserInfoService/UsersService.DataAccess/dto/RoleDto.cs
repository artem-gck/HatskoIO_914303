using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.DataAccess.dto
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserDto>? User { get; set; }
    }
}
