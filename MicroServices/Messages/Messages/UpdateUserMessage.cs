namespace Messages
{
    public class UpdateUserMessage
    {
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid PositionId { get; set; }
    }
}
