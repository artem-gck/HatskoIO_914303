using AutoMapper;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Ports.Output;
using TaskCrudService.Posts.DataSource;

namespace TaskCrudService.Adapters.Output
{
    public class TaskService : IService<TaskEntity>
    {
        private readonly IRepository<TaskEntity> _taskRepository;

        public TaskService(IRepository<TaskEntity> taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<Guid> AddAsync(TaskEntity entity)
        {
            var result = await _taskRepository.AddAsync(entity);

            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _taskRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            var listOfTask = await _taskRepository.GetAllAsync();

            return listOfTask;
        }

        public async Task<TaskEntity> GetAsync(Guid id)
        {
            var taskDto = await _taskRepository.GetAsync(id);

            return taskDto;
        }

        public async Task<IEnumerable<TaskEntity>> GetByNameAync(Guid id)
        {
            var listOfTasks = await _taskRepository.GetByNameId(id);

            return listOfTasks;
        }

        public async Task UpdateAsync(Guid id, TaskEntity entity)
        {
            await _taskRepository.UpdateAsync(id, entity);
        }
    }
}
