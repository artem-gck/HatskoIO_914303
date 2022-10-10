using StructureService.Domain.Entities;

namespace StructureService.Application.Services
{
    public interface IUserService
    {
        public Task<UserEntity> GetAsync(Guid id);
        public Task<Guid> AddAsync(UserEntity entity);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Guid id, UserEntity entity);
    }
}
