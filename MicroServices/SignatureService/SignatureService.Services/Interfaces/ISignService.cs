namespace SignatureService.Services.Interfaces
{
    public interface ISignService
    {
        public Task AddAsync(Guid userId, Guid documentId, int version);
        public Task<IEnumerable<Guid>> GetUsersByDocumentIdAsync(Guid id, int version);
        public Task<bool> CheckDocumentByUserAsync(Guid userId, Guid documentId, int version);
    }
}
