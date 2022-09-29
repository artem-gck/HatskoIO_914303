using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLoginService.Domain.Exceptions
{
    public class UserLoginNotFoundException : Exception
    {
        public UserLoginNotFoundException(int id)
            : base($"No userLogin with id = {id}")
        {

        }
    }
}
