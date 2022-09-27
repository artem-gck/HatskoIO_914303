using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.DataAccess.Entities
{
    public class PositionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserInfoEntity>? UserInfo { get; set; }
    }
}
