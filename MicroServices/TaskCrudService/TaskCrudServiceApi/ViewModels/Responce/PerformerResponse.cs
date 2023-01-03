namespace TaskCrudServiceApi.ViewModels.Responce
{
    public class PerformerResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? TypeOfTask { get; set; }
        public string? Description { get; set; }
        public byte[]? PublicKey { get; set; }
        public Guid? SignatureDocumentId { get; set; }
        public string? Resolve { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
