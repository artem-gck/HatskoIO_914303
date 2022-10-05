using TaskCrudService.Domain.Entities;

namespace TaskCrudService.Posts.DataSource
{
    public interface IUserRepository
    {
        public Task<IEnumerable<TaskEntity>> GetAllAsync(Guid id); 
    }
}
