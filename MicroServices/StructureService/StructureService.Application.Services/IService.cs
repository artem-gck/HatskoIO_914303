using StructureService.Application.Services.Dto;

namespace StructureService.Application.Services
{
    public interface IService<T> where T : BaseDto
    {
        public Task<T> GetAsync(Guid id);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<Guid> AddAsync(T entity);
        public Task<Guid> DeleteAsync(Guid id);
        public Task<Guid> UpdateAsync(Guid id, T entity);
    }
}
