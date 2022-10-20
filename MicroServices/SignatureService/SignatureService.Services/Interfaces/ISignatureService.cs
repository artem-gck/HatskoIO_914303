namespace SignatureService.Services.Interfaces
{
    public interface ISignatureService
    {
        public Task AddAsync(Guid userId, Guid documentId, int version);
        public Task<IEnumerable<Guid>> GetUsersByDocumentIdAsync(Guid id, int version);
        public Task<bool> CheckDocumentByUser(Guid userId, Guid documentId, int version);
    }
}
