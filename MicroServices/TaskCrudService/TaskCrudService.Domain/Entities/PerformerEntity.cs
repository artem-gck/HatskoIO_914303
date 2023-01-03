namespace TaskCrudService.Domain.Entities
{
    public class PerformerEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public string? TypeOfTask { get; set; }
        public string? Description { get; set; }
        public byte[]? PublicKey { get; set; }
        public Guid? SignatureDocumentId { get; set; }
        public string? Resolve { get; set; }
        public bool? IsCompleted { get; set; }
        public Guid? TaskId { get; set; }
        public TaskEntity? Task { get; set; }
    }
}
