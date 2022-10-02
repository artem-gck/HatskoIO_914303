using Microsoft.EntityFrameworkCore;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Domain.Exceptions;
using TaskCrudService.Domain.Realisation.Context;
using TaskCrudService.Domain.Services;

namespace TaskCrudService.Domain.Realisation
{
    public class TasksRepository : IRepository<TaskEntity>
    {
        private readonly TaskContext _taskContext;

        public TasksRepository(TaskContext taskContext)
        {
            _taskContext = taskContext ?? throw new ArgumentNullException(nameof(taskContext));
        }

        public async Task<Guid> AddAsync(TaskEntity entity)
        {
            entity.Type = await GetType(entity.Type.Name);

            for (var i = 0; i < entity.Arguments.Count; i++)
                entity.Arguments[i].ArgumentType = await GetArgumentType(entity.Arguments[i].ArgumentType.Name);

            var taskEntity = _taskContext.Tasks.Add(entity);

            await _taskContext.SaveChangesAsync();

            return taskEntity.Entity.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var taskEntity = await _taskContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (taskEntity is null)
                throw new NotFoundException<TaskEntity>(id);

            _taskContext.Tasks.Remove(taskEntity);

            await _taskContext.SaveChangesAsync();

            return id;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            var listOfTaskEntity = await _taskContext.Tasks.Include(t => t.Type)
                                                           .Include(t => t.Arguments)
                                                               .ThenInclude(ar => ar.ArgumentType)
                                                           .ToListAsync();

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

            return taskEntity;
        }

        public async Task<Guid> UpdateAsync(Guid id, TaskEntity entity)
        {
            var taskEntity = await _taskContext.Tasks.Include(t => t.Type)
                                                     .Include(t => t.Arguments)
                                                         .ThenInclude(ar => ar.ArgumentType)
                                                     .FirstOrDefaultAsync(t => t.Id == id);

            if (taskEntity is null)
                throw new NotFoundException<TaskEntity>(id);

            entity.Id = id;

            _taskContext.Tasks.Update(entity);

            await _taskContext.SaveChangesAsync();

            return id;
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
