using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskCrudService.Application.Services;
using TaskCrudService.Application.Services.Dto;
using TaskCrudService.Domain.Services;

namespace TaskCrudService.Application.Realisation
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
