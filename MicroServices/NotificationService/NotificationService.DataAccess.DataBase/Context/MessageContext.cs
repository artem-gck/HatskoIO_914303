using Microsoft.EntityFrameworkCore;
using NotificationService.DataAccess.DataBase.Entity;

namespace NotificationService.DataAccess.DataBase.Context
{
    public class MessageContext : DbContext
    {
        public DbSet<MessageEntity> Messages { get; set; } = null!;

        public MessageContext(DbContextOptions<MessageContext> options)
            : base(options)
        {

        }
    }
}
