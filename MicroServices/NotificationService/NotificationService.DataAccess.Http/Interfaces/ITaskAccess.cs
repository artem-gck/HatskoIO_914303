using NotificationService.DataAccess.Http.Entity;

namespace NotificationService.DataAccess.Http.Interfaces
{
    public interface ITaskAccess
    {
        public Task<IEnumerable<TaskResponce>> GetTasksAsync();
    }
}
