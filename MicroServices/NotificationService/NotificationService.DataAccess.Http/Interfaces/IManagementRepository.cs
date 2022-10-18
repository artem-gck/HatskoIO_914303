using NotificationService.DataAccess.Http.Entity;

namespace NotificationService.DataAccess.Http.Interfaces
{
    public interface IManagementRepository
    {
        public Task<UserResponce> GetUserInfoAsync(Guid id);
    }
}
