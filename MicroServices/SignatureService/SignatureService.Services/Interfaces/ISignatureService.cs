namespace SignatureService.Services.Interfaces
{
    public interface ISignatureService
    {
        public Task AddAsync(Guid userId, Guid documentId, int version);
        public Task GetByDocumentIdAsync(Guid id, int version);
    }
}
