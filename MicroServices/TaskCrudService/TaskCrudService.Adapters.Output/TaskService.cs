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

        public async Task<IEnumerable<TaskEntity>> GetAsync()
        {
            var listOfTask = await _taskRepository.GetAsync();

            return listOfTask;
        }

        public async Task<TaskEntity> GetAsync(Guid id)
        {
            var taskDto = await _taskRepository.GetAsync(id);

            return taskDto;
        }

        public async Task<IEnumerable<TaskEntity>> GetByNameAync(Guid id, string status)
        {
            if (status is null)
            {
                var listOfTasks = await _taskRepository.GetByNameId(id);
                return listOfTasks;
            }

            var listOfTasksByStatus = await _taskRepository.GetByNameId(id, status);

            return listOfTasksByStatus;
        }

        public async Task<IEnumerable<TaskEntity>> GetByPerformerId(Guid id, string status)
            => await _taskRepository.GetByPerformerId(id, status);

        public async Task UpdateAsync(Guid id, TaskEntity entity)
        {
            await _taskRepository.UpdateAsync(id, entity);
        }
    }
}
