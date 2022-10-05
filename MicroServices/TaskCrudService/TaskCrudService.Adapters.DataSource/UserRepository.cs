using Microsoft.EntityFrameworkCore;
using TaskCrudService.Adapters.DataSource.Context;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Posts.DataSource;

namespace TaskCrudService.Adapters.DataSource
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskContext _taskContext;

        public UserRepository(TaskContext taskContext)
        {
            _taskContext = taskContext ?? throw new ArgumentNullException(nameof(taskContext));
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync(Guid id)
        {
            var listOfTaskByUser = await _taskContext.Tasks.Include(t => t.Type)
                                                           .Include(t => t.Arguments)
                                                               .ThenInclude(ar => ar.ArgumentType)
                                                           .Where(t => t.OwnerUserId == id)
                                                           .ToListAsync();

            return listOfTaskByUser;
        }
    }
}
