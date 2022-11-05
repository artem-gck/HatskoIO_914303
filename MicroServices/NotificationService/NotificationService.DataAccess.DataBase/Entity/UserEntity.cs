using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.DataAccess.DataBase.Entity
{
    public class UserEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
