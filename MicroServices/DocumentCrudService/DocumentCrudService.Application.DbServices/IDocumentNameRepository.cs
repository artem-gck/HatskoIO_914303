using DocumentCrudService.Repositories.Entities;

namespace DocumentCrudService.Repositories.DbServices
{
    public interface IDocumentNameRepository
    {
        public Task<IEnumerable<DocumentNameEntity>> GetAllAsync();
        public Task<DocumentEntity> GetByNameAsync(string fileName, int version = -1);
    }
}
