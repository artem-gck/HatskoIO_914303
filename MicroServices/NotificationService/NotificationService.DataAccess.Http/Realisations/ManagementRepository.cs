using NotificationService.DataAccess.Http.Entity;
using NotificationService.DataAccess.Http.Interfaces;

namespace NotificationService.DataAccess.Http.Realisations
{
    public class ManagementRepository : IManagementRepository
    {
        public Task<UserResponce> GetUserInfoAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
