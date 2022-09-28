using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLoginService.Domain.ViewModels
{
    public class UserLoginViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int? TockenId { get; set; }
        public int? UserInfoId { get; set; }
    }
}
