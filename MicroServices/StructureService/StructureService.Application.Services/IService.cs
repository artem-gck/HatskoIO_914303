using StructureService.Application.Services.Dto;

namespace StructureService.Application.Services
{
    public interface IService<T> where T : BaseDto
    {
        public Task<T> GetAsync(int id);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<int> AddAsync(T entity);
        public Task<int> DeleteAsync(int id);
        public Task<int> UpdateAsync(int id, T entity);
    }
}
