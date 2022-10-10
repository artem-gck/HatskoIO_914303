using DocumentCrudService.Repositories.Entities;

namespace DocumentCrudService.Repositories.DbServices
{
    public interface IDocumentRepository
    {
        public Task<DocumentEntity> GetAsync(string id);
        public Task<DocumentEntity> GetByNameAsync(string fileName, int version = -1);
        public Task DeleteAsync(string id);
        public Task<string> AddAsync(byte[] document, string fileName);
        public Task UpdateAsync(byte[] document, string fileName);
    }
}
