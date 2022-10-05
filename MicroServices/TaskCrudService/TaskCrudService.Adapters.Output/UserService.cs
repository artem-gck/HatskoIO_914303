using AutoMapper;
using TaskCrudService.Ports.Output;
using TaskCrudService.Ports.Output.Dto;
using TaskCrudService.Posts.DataSource;

namespace TaskCrudService.Adapters.Output
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper; 

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<TaskDto>> GetAllAsync(Guid id)
        {
            var listOfTaskByUser = _mapper.Map<IEnumerable<TaskDto>>(await _userRepository.GetAllAsync(id));

            return listOfTaskByUser;
        }
    }
}
