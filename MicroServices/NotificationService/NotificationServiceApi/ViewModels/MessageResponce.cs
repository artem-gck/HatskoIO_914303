namespace NotificationServiceApi.ViewModels
{
    public class MessageResponce
    {
        public Guid Id { get; set; }
        public Guid RecipientId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
