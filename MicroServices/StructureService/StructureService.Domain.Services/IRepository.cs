using StructureService.Domain.Entities;

namespace StructureService.Domain.Services
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Task<T> GetAsync(int id);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<int> AddAsync(T entity);
        public Task<int> DeleteAsync(int id);
        public Task<int> UpdateAsync(int id, T entity);
    }
}
