using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.DataAccess.Entities
{
    public class UserInfoEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
        public PositionEntity Position { get; set; }
    }
}
