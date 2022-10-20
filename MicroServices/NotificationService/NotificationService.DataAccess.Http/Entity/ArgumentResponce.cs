using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.DataAccess.Http.Entity
{
    public class ArgumentResponce
    {
        public Guid Id { get; set; }
        public string ArgumentType { get; set; }
        public string Value { get; set; }
    }
}
