using TaskCrudService.Domain.Entities;

namespace TaskCrudService.Posts.DataSource
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Task<T> GetAsync(Guid id);
        public Task<IEnumerable<T>> GetByNameId(Guid id);
        public Task<IEnumerable<T>> GetAsync();
        public Task<Guid> AddAsync(T entity);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Guid id, T entity);
    }
}
