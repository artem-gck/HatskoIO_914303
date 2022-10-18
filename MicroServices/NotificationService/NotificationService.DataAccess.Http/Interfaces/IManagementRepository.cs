using NotificationService.DataAccess.Http.Entity;

namespace NotificationService.DataAccess.Http.Interfaces
{
    internal interface IManagementRepository
    {
        public Task<UserResponce> GetUserInfoAsync(Guid id);
    }
}
