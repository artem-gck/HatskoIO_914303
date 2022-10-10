using DocumentCrudService.Repositories.Entities;

namespace DocumentCrudService.Repositories.DbServices
{
    public interface IDocumentNameRepository
    {
        public Task<IEnumerable<DocumentNameEntity>> GetAsync();
    }
}
