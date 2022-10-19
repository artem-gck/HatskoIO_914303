﻿namespace NotificationService.DataAccess.DataBase.Entity
{
    public class MessageEntity
    {
        public Guid Id { get; set; }
        public Guid RecipientId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
