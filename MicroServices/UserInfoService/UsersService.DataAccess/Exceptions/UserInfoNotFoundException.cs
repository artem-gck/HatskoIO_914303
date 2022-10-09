using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.DataAccess.Exceptions
{
    public class UserInfoNotFoundException : Exception
    {
        public UserInfoNotFoundException(Guid id) 
            : base($"No userInfo with id = {id}")
        {

        }
    }
}
