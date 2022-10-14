using AutoMapper;
using StructureService.Application.Services;
using StructureService.Domain.Services;
using Microsoft.Extensions.Logging;
using StructureService.Domain.Entities;

namespace StructureService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IEnumerable<UserEntity>> GetByDepartmentId(Guid departmentId)
            => await _userRepository.GetByDepartmentId(departmentId);

        public async Task<Guid> AddAsync(Guid departmentId, UserEntity entity)
            => await _userRepository.AddAsync(departmentId, entity);

        public async Task DeleteAsync(Guid departmentId, Guid userId)
            => await _userRepository.DeleteAsync(departmentId, userId);

        public async Task<UserEntity> GetAsync(Guid departmentId, Guid userId)
            => await _userRepository.GetAsync(departmentId, userId);

        public async Task UpdateAsync(Guid departmentId, Guid userId, UserEntity entity)
            => await _userRepository.UpdateAsync(departmentId, userId, entity);
    }
}
