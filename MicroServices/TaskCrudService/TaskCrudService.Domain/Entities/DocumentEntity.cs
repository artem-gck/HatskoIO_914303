namespace TaskCrudService.Domain.Entities
{
    public class DocumentEntity : BaseEntity
    {
        public Guid DocumentId { get; set; }
        public Guid? TaskId { get; set; }
        public TaskEntity? Task { get; set; }
    }
}
