using DocumentCrudService.Repositories.Entities;

namespace DocumentCrudService.Repositories.DbServices
{
    public interface IDocumentRepository
    {
        public Task<DocumentEntity> GetAsync(Guid id, int version);
        public Task DeleteAsync(Guid id);
        public Task<Guid> AddAsync(Guid createrId, byte[] document, string fileName);
        public Task UpdateAsync(Guid id, Guid createrId, byte[] document, string fileName);
    }
}
