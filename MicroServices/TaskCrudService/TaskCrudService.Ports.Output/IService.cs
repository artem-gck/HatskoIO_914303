using TaskCrudService.Domain.Entities;

namespace TaskCrudService.Ports.Output
{
    public interface IService<T> where T : BaseEntity
    {
        public Task<T> GetAsync(Guid id);
        public Task<IEnumerable<T>> GetByNameAync(Guid id, string status);
        public Task<IEnumerable<T>> GetByPerformerId(Guid id, string status);
        public Task<IEnumerable<T>> GetAsync();
        public Task<Guid> AddAsync(T entity);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Guid id, T entity);
    }
}
