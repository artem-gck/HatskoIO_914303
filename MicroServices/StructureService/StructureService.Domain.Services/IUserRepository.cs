using StructureService.Domain.Entities;

namespace StructureService.Domain.Services
{
    public interface IUserRepository
    {
        public Task<User> GetAsync(Guid id);
        public Task<Guid> AddAsync(User entity);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Guid id, User entity);
    }
}
