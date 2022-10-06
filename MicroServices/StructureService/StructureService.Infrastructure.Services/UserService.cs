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
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DepartmentUnitEntity> GetAsync(Guid id)
        {
            var dto = _mapper.Map<DepartmentUnitEntity>(await _userRepository.GetAsync(id));

            _logger.LogDebug("Geted DepartmentUnit from db = {@Dto}", dto);

            return dto;
        }
    }
}
