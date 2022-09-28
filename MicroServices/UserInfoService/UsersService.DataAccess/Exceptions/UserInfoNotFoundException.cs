using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersService.DataAccess.Exceptions
{
    /// <summary>
    /// Custom exception when no user info with specific id. 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class UserInfoNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoNotFoundException"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public UserInfoNotFoundException(int id) 
            : base($"No userInfo with id = {id}")
        {

        }
    }
}
