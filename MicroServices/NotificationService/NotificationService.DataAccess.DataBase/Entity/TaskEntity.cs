using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.DataAccess.DataBase.Entity
{
    public class TaskEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime DeadLine { get; set; }
        public string Header { get; set; }
    }
}
