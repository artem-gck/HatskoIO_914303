using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.DataAccess.Http.Entity
{
    public class TaskResponce
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Header { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime DeadLine { get; set; }
        public List<ArgumentResponce>? Arguments { get; set; }
    }
}
