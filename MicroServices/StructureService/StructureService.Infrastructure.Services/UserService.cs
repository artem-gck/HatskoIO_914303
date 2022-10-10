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

        public async Task<Guid> AddAsync(User entity)
            => await _userRepository.AddAsync(entity);

        public async Task DeleteAsync(Guid id)
            => await _userRepository.DeleteAsync(id);

        public async Task<User> GetAsync(Guid id)
            => await _userRepository.GetAsync(id);

        public async Task UpdateAsync(Guid id, User entity)
            => await _userRepository.UpdateAsync(id, entity);
    }
}
