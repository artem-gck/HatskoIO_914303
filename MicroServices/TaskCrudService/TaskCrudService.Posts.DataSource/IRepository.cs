using TaskCrudService.Domain.Entities;

namespace TaskCrudService.Posts.DataSource
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Task<T> GetAsync(Guid id);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<Guid> AddAsync(T entity);
        public Task<Guid> DeleteAsync(Guid id);
        public Task<Guid> UpdateAsync(Guid id, T entity);
    }
}
