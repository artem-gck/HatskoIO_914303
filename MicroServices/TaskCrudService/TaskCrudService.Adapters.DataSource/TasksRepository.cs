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
            _logger.Debug("qwe");

            entity.Type = await GetType(entity.Type.Name);

            for (var i = 0; i < entity.Arguments.Count; i++)
                entity.Arguments[i].ArgumentType = await GetArgumentType(entity.Arguments[i].ArgumentType.Name);

            var taskEntity = _taskContext.Tasks.Add(entity);
            await _taskContext.SaveChangesAsync();

            _logger.Debug("Add entity to db {id}", taskEntity.Entity.Id);

            return taskEntity.Entity.Id;
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

        public async Task<IEnumerable<TaskEntity>> GetByNameId(Guid id)
        {
            var listOfTaskByUser = await _taskContext.Tasks.Include(t => t.Type)
                                               .Include(t => t.Arguments)
                                                   .ThenInclude(ar => ar.ArgumentType)
                                               .Where(t => t.OwnerUserId == id)
                                               .ToListAsync();

            return listOfTaskByUser;
        }

        public async Task UpdateAsync(Guid id, TaskEntity entity)
        {
            entity.Id = id;

            _taskContext.Tasks.Update(entity);
            await _taskContext.SaveChangesAsync();

            _logger.Debug("Update entity in db {id}", id);
        }

        private async Task<TypeEntity> GetType(string name)
        {
            var typeEntity = await _taskContext.Types.FirstOrDefaultAsync(t => t.Name == name);

            if (typeEntity is null)
            {
                typeEntity = new TypeEntity()
                {
                    Name = name
                };

                var entity = _taskContext.Types.Add(typeEntity);
                await _taskContext.SaveChangesAsync();

                return entity.Entity;
            }

            return typeEntity;
        }

        private async Task<ArgumentTypeEntity> GetArgumentType(string name)
        {
            var argumentTypeEntity = await _taskContext.ArgumentTypes.FirstOrDefaultAsync(t => t.Name == name);

            if (argumentTypeEntity is null)
            {
                argumentTypeEntity = new ArgumentTypeEntity()
                {
                    Name = name
                };

                var entity = _taskContext.ArgumentTypes.Add(argumentTypeEntity);
                await _taskContext.SaveChangesAsync();

                return entity.Entity;
            }

            return argumentTypeEntity;
        }
    }
}
