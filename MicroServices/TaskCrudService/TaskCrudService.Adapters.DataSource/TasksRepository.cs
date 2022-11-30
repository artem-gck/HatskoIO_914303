using Microsoft.EntityFrameworkCore;
using NLog;
using TaskCrudService.Adapters.DataSource.Context;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Domain.Exceptions;
using TaskCrudService.Posts.DataSource;

namespace TaskCrudService.Adapters.DataSource
{
    public class TasksRepository : IRepository<TaskEntity>
    {
        private readonly TaskContext _taskContext;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public TasksRepository(TaskContext taskContext)
        {
            _taskContext = taskContext ?? throw new ArgumentNullException(nameof(taskContext));
        }

        public async Task<Guid> AddAsync(TaskEntity entity)
        {
            entity.TypeId = await GetType(entity.Type.Name);

            for (var i = 0; i < entity.Arguments.Count; i++)
                entity.Arguments[i].ArgumentTypeId = await GetArgumentType(entity.Arguments[i].ArgumentType.Name);

            var taskEntity = new TaskEntity()
            {
                TypeId = entity.TypeId,
                Header = entity.Header,
                OwnerUserId = entity.OwnerUserId,
                DeadLine = entity.DeadLine,
                Status = "For work",
                CreatedAt = DateTime.Now
            };

            var taskEntity1 = _taskContext.Tasks.Add(taskEntity);
            await _taskContext.SaveChangesAsync();

            for (var i = 0; i < entity.Arguments.Count; i++)
                await GetArgument(entity.Arguments[i], taskEntity1.Entity.Id);

            _logger.Debug("Add entity to db {id}", taskEntity1.Entity.Id);

            return taskEntity1.Entity.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var taskEntity = await _taskContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (taskEntity is null)
                throw new NotFoundException<TaskEntity>(id);

            _taskContext.Tasks.Remove(taskEntity);
            await _taskContext.SaveChangesAsync();

            _logger.Debug("Delete entity from db {id}", id);
        }

        public async Task<IEnumerable<TaskEntity>> GetAsync()
        {
            var listOfTaskEntity = await _taskContext.Tasks.Include(t => t.Type)
                                                           .Include(t => t.Arguments)
                                                               .ThenInclude(ar => ar.ArgumentType)
                                                           .ToListAsync();

            var listOfId = string.Join(", ", listOfTaskEntity.Select(en => en.Id.ToString()));

            _logger.Debug("Get entities from db {listOfId}", listOfId);

            return listOfTaskEntity;
        }

        public async Task<TaskEntity> GetAsync(Guid id)
        {
            var taskEntity = await _taskContext.Tasks.Include(t => t.Type)
                                                     .Include(t => t.Arguments)
                                                         .ThenInclude(ar => ar.ArgumentType)
                                                     .FirstOrDefaultAsync(t => t.Id == id);

            if (taskEntity is null)
                throw new NotFoundException<TaskEntity>(id);

            _logger.Debug("Get entity from db {id}", taskEntity.Id);

            return taskEntity;
        }

        public async Task<IEnumerable<TaskEntity>> GetByNameId(Guid id, string status)
        {
            var listOfTaskByUser = await _taskContext.Tasks
                                                     .Include(t => t.Type)
                                                     .Include(t => t.Arguments)
                                                         .ThenInclude(ar => ar.ArgumentType)
                                                     .Where(t => t.OwnerUserId == id && t.Status == status)
                                                     .ToListAsync();

            return listOfTaskByUser;
        }

        public async Task<IEnumerable<TaskEntity>> GetByNameId(Guid id)
        {
            var listOfTaskByUser = await _taskContext.Tasks
                                                     .Include(t => t.Type)
                                                     .Include(t => t.Arguments)
                                                         .ThenInclude(ar => ar.ArgumentType)
                                                     .Where(t => t.OwnerUserId == id)
                                                     .ToListAsync();

            return listOfTaskByUser;
        }

        public async Task<IEnumerable<TaskEntity>> GetByPerformerId(Guid id, string status)
        {
            var listOfTaskByUser = await _taskContext.Tasks
                                                     .Include(t => t.Type)
                                                     .Include(t => t.Arguments)
                                                         .ThenInclude(ar => ar.ArgumentType)
                                                     .Where(t => t.Arguments.Count(arg => arg.Value == id.ToString()) > 0 && t.Status == status)
                                                     .ToListAsync();

            return listOfTaskByUser;
        }

        public async Task UpdateAsync(Guid id, TaskEntity entity)
        {
            var createdDate = entity.CreatedAt;

            entity.Id = id;

            _taskContext.Tasks.Remove(entity);

            entity.TypeId = await GetType(entity.Type.Name);

            for (var i = 0; i < entity.Arguments.Count; i++)
                entity.Arguments[i].ArgumentTypeId = await GetArgumentType(entity.Arguments[i].ArgumentType.Name);

            var taskEntity = new TaskEntity()
            {
                TypeId = entity.TypeId,
                Header = entity.Header,
                OwnerUserId = entity.OwnerUserId,
                DeadLine = entity.DeadLine,
                Status = "For work",
                CreatedAt = createdDate
            };

            var taskEntity1 = _taskContext.Tasks.Add(taskEntity);
            await _taskContext.SaveChangesAsync();

            for (var i = 0; i < entity.Arguments.Count; i++)
                await GetArgument(entity.Arguments[i], taskEntity1.Entity.Id);

            _logger.Debug("Update entity in db {id}", id);
        }

        private async Task<Guid> GetType(string name)
        {
            var typeEntity = new TypeEntity()
            {
                Name = name
            };

            var entity = _taskContext.Types.Add(typeEntity);
            await _taskContext.SaveChangesAsync();

            return entity.Entity.Id;
        }

        private async Task<Guid> GetArgumentType(string name)
        {
            var argumentTypeEntity = new ArgumentTypeEntity()
            {
                Name = name
            };

            var entity = _taskContext.ArgumentTypes.Add(argumentTypeEntity);
            await _taskContext.SaveChangesAsync();

            return entity.Entity.Id;
        }

        private async Task<Guid> GetArgument(ArgumentEntity entity, Guid taskId)
        {
            var argumentEntity = new ArgumentEntity()
            {
                Value = entity.Value,
                ArgumentTypeId = entity.ArgumentTypeId,
                TaskId = taskId
            };

            var argumentEntity1 = _taskContext.Arguments.Add(argumentEntity);
            await _taskContext.SaveChangesAsync();

            return argumentEntity1.Entity.Id;
        }
    }
}
