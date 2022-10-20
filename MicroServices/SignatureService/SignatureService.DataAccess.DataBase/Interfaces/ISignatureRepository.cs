using SignatureService.DataAccess.DataBase.Entities;

namespace SignatureService.DataAccess.DataBase.Interfaces
{
    public interface ISignatureRepository
    {
        public Task AddAsync(SignatureEntity entity);
        public Task<SignatureEntity> GetByDocumentIdAsync(Guid id, int version);
    }
}
