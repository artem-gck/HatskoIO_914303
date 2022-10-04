using DocumentCrudService.Domain.Entities;

namespace DocumentCrudService.Application.DbServices
{
    public interface IDocumentRepository
    {
        public Task<DocumentEntity> GetAsync(string id);
        public Task<string> DeleteAsync(string id);
        public Task<string> AddAsync(byte[] document, string fileName);
        public Task<string> UpdateAsync(byte[] document, string fileName);
    }
}
