using AutoMapper;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Ports.Output;
using TaskCrudService.Ports.Output.Dto;
using TaskCrudService.Posts.DataSource;

namespace TaskCrudService.Adapters.Output
{
    public class TaskService : IService<TaskDto>
    {
        private readonly IRepository<TaskEntity> _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(IRepository<TaskEntity> taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Guid> AddAsync(TaskDto dto)
        {
            var result = await _taskRepository.AddAsync(_mapper.Map<TaskEntity>(dto));

            return result;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var result = await _taskRepository.DeleteAsync(id);

            return result;
        }

        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            var listOfTask = (await _taskRepository.GetAllAsync()).Select(t => _mapper.Map<TaskDto>(t));

            return listOfTask;
        }

        public async Task<TaskDto> GetAsync(Guid id)
        {
            var taskDto = _mapper.Map<TaskDto>(await _taskRepository.GetAsync(id));

            return taskDto;
        }

        public async Task<Guid> UpdateAsync(Guid id, TaskDto dto)
        {
            var result = await _taskRepository.UpdateAsync(id, _mapper.Map<TaskEntity>(dto));

            return result;
        }
    }
}
