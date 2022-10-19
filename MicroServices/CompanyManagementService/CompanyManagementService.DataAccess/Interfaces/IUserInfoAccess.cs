using CompanyManagementService.DataAccess.UserEntity;

namespace CompanyManagementService.DataAccess.Interfaces
{
    public interface IUserInfoAccess
    {
        public Task<UserResponce> GetAsync(Guid id);
        public Task<IEnumerable<UserResponce>> GetByDepartmentIdAsync(Guid id);
        public Task<IEnumerable<UserResponce>> GetAsync();
        public Task DeleteAsync(Guid id);
        public Task<Guid> PostAsync(AddUserRequest addUserRequest);
        public Task PutAsync(Guid id, UpdateUserRequest updateUserRequest);
    }
}
