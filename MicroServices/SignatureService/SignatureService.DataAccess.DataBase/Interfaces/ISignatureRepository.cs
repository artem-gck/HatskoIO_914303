using SignatureService.DataAccess.DataBase.Entities;

namespace SignatureService.DataAccess.DataBase.Interfaces
{
    public interface ISignatureRepository
    {
        public Task AddAsync(SignatureEntity entity);
        public Task<IEnumerable<SignatureEntity>> GetByDocumentIdAsync(Guid id, int version);
        public Task<SignatureEntity> GetSignatureAync(Guid userId, Guid documentId, int version);
    }
}
