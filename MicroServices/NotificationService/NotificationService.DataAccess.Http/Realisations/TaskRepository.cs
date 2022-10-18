using NotificationService.DataAccess.Http.Entity;
using NotificationService.DataAccess.Http.Interfaces;

namespace NotificationService.DataAccess.Http.Realisations
{
    public class TaskRepository : ITaskRepository
    {
        public Task<IEnumerable<TaskResponce>> GetTasksAsync()
        {
            throw new NotImplementedException();
        }
    }
}
