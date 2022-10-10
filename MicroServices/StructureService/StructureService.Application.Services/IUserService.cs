using StructureService.Domain.Entities;

namespace StructureService.Application.Services
{
    public interface IUserService
    {
        public Task<User> GetAsync(Guid id);
        public Task<Guid> AddAsync(User entity);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Guid id, User entity);
    }
}
