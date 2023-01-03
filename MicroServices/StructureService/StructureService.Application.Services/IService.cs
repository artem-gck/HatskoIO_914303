using StructureService.Domain.Entities;

namespace StructureService.Application.Services
{
    public interface IService<T> where T : BaseEntity
    {
        public Task<T> GetAsync(Guid id);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetAllAsync(int page, int count);
        public Task<Guid> AddAsync(T entity);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Guid id, T entity);
    }
}
