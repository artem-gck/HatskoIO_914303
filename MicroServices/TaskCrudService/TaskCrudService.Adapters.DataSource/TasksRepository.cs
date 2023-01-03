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

            for (var i = 0; i < entity.Performers.Count; i++)
            {
                entity.Performers[i].Task = taskEntity1.Entity;
                entity.Performers[i].TaskId = taskEntity1.Entity.Id;
                entity.Performers[i].IsCompleted = false;
            }

            for (var i = 0; i < entity.Documents.Count; i++)
            {
                entity.Documents[i].Task = taskEntity1.Entity;
                entity.Documents[i].TaskId = taskEntity1.Entity.Id;
            }

            _taskContext.Performers.AddRange(entity.Performers);
            _taskContext.Documents.AddRange(entity.Documents);

            await _taskContext.SaveChangesAsync();

            _logger.Debug("Add entity to db {id}", taskEntity1.Entity.Id);

            return taskEntity1.Entity.Id.Value;
        }

        public async Task DeleteAsync(Guid id)
        {
            var taskEntity = await _taskContext.Tasks.Include(t => t.Type)
                                                     .Include(t => t.Performers)
                                                     .Include(t => t.Documents)
                                                     .FirstOrDefaultAsync(t => t.Id == id);

            if (taskEntity is null)
                throw new NotFoundException<TaskEntity>(id);

            _taskContext.Tasks.Remove(taskEntity);
            await _taskContext.SaveChangesAsync();

            _logger.Debug("Delete entity from db {id}", id);
        }

        public async Task<IEnumerable<TaskEntity>> GetAsync()
        {
            var listOfTaskEntity = await _taskContext.Tasks.Include(t => t.Type)
                                                           .Include(t => t.Performers)
                                                           .Include(t => t.Documents)
                                                           .OrderByDescending(t => t.CreatedAt)
                                                           .ToListAsync();

            var listOfId = string.Join(", ", listOfTaskEntity.Select(en => en.Id.ToString()));

            _logger.Debug("Get entities from db {listOfId}", listOfId);

            return listOfTaskEntity;
        }

        public async Task<TaskEntity> GetAsync(Guid id)
        {
            var taskEntity = await _taskContext.Tasks.Include(t => t.Type)
                                                     .Include(t => t.Performers)
                                                     .Include(t => t.Documents)
                                                     .OrderByDescending(t => t.CreatedAt)
                                                     .FirstOrDefaultAsync(t => t.Id == id);

            if (taskEntity is null)
                throw new NotFoundException<TaskEntity>(id);

            _logger.Debug("Get entity from db {id}", taskEntity.Id);

            return taskEntity;
        }

        public async Task<IEnumerable<TaskEntity>> GetByNameId(Guid id, string status)
        {
            var listOfTaskByUser = await _taskContext.Tasks.Include(t => t.Type)
                                                           .Include(t => t.Performers)
                                                           .Include(t => t.Documents)
                                                           .Where(t => t.OwnerUserId == id && t.Status == status)
                                                           .OrderByDescending(t => t.CreatedAt)
                                                           .ToListAsync();

            return listOfTaskByUser;
        }

        public async Task<IEnumerable<TaskEntity>> GetByNameId(Guid id)
        {
            var listOfTaskByUser = await _taskContext.Tasks.Include(t => t.Type)
                                                           .Include(t => t.Performers)
                                                           .Include(t => t.Documents)
                                                           .Where(t => t.OwnerUserId == id)
                                                           .OrderByDescending(t => t.CreatedAt)
                                                           .ToListAsync();

            return listOfTaskByUser;
        }

        public async Task<IEnumerable<TaskEntity>> GetByPerformerId(Guid id, string status)
        {
            var listOfTaskByUser = await _taskContext.Tasks.Include(t => t.Type)
                                                           .Include(t => t.Performers)
                                                           .Include(t => t.Documents)
                                                           .Where(t => t.Performers.Count(perf => perf.UserId == id) > 0 && t.Status == status)
                                                           .OrderByDescending(t => t.CreatedAt)
                                                           .ToListAsync();

            List<TaskEntity> tasks = new List<TaskEntity>();

            foreach (var task in listOfTaskByUser)
            {
                var isT = true;

                if (task.Type.Name != "parallel")
                    foreach (var perf in task.Performers)
                    {
                        if (perf.UserId == id || !isT)
                            break;

                        isT = perf.IsCompleted.Value;
                    }

                if (isT)
                    tasks.Add(task);
            }

            return tasks;
        }

        public async Task UpdateAsync(Guid id, TaskEntity entity)
        {
            var taskEntity = await _taskContext.Tasks.Include(t => t.Type)
                                                     .Include(t => t.Performers)
                                                     .Include(t => t.Documents)
                                                     .FirstOrDefaultAsync(t => t.Id == id);

            _taskContext.Documents.RemoveRange(taskEntity.Documents);

            for (var i = 0; i < entity.Documents.Count; i++)
            {
                entity.Documents[i].Id = Guid.NewGuid();
                entity.Documents[i].Task = taskEntity;
            }

            _taskContext.Documents.AddRange(entity.Documents);

            _taskContext.Performers.RemoveRange(taskEntity.Performers);

            for (var i = 0; i < entity.Performers.Count; i++)
            {
                entity.Performers[i].Id = Guid.NewGuid();
                entity.Performers[i].Task = taskEntity;
            }

            _taskContext.Performers.AddRange(entity.Performers);

            taskEntity.Type = entity.Type;
            taskEntity.Header = entity.Header;
            taskEntity.Status = entity.Status;
            taskEntity.DeadLine = entity.DeadLine;

            if (entity.Performers.All(p => p.IsCompleted.Value))
                taskEntity.Status = "Completed";

            await _taskContext.SaveChangesAsync();

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

            return entity.Entity.Id.Value;
        }
    }
}
