using TaskCrudService.Ports.Output.Dto;

namespace TaskCrudService.Ports.Output
{
    public interface IUserService
    {
        public Task<IEnumerable<TaskDto>> GetAllAsync(Guid id);
    }
}
