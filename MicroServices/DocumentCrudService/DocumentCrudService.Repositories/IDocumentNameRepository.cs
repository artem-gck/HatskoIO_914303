using DocumentCrudService.Repositories.Entities;

namespace DocumentCrudService.Repositories.DbServices
{
    public interface IDocumentNameRepository
    {
        public Task<IEnumerable<DocumentNameEntity>> GetAsync(int numberOfPage, int countOnPage);
        public Task<IEnumerable<DocumentNameEntity>> GetByUserIdAsync(Guid userId);
    }
}
