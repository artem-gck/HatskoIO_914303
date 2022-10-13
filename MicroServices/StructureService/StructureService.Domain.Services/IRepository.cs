using StructureService.Domain.Entities;

namespace StructureService.Domain.Services
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Task<T> GetAsync(Guid id);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<Guid> AddAsync(T entity);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Guid id, T entity);
    }
}
