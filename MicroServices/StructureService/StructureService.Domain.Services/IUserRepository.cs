using StructureService.Domain.Entities;

namespace StructureService.Domain.Services
{
    public interface IUserRepository
    {
        public Task<UserEntity> GetAsync(Guid id);
        public Task<Guid> AddAsync(UserEntity entity);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Guid id, UserEntity entity);
    }
}
