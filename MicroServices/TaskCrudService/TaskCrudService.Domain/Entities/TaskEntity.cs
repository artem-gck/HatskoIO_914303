namespace TaskCrudService.Domain.Entities
{
    public class TaskEntity : BaseEntity
    {
        public TypeEntity Type { get; set; }
        public string Header { get; set; }
        public Guid OwnerUserId { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ArgumentEntity>? Arguments { get; set; }
    }
}
