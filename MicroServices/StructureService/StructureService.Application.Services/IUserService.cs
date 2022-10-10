using StructureService.Domain.Entities;

namespace StructureService.Application.Services
{
    public interface IUserService
    {
        public Task<UserEntity> GetAsync(Guid departmentId, Guid userId);
        public Task<Guid> AddAsync(Guid departmentId, UserEntity entity);
        public Task DeleteAsync(Guid departmentId, Guid userId);
        public Task UpdateAsync(Guid departmentId, Guid userId, UserEntity entity);
    }
}
