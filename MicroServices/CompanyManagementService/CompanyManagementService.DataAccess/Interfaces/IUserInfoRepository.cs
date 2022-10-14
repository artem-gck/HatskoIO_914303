using CompanyManagementService.DataAccess.UserEntity;

namespace CompanyManagementService.DataAccess.Interfaces
{
    public interface IUserInfoRepository
    {
        public Task<UserResponce> Get(Guid id);
        public Task<IEnumerable<UserResponce>> GetByDepartmentId(Guid id);
        public Task<IEnumerable<UserResponce>> Get();
        public Task Delete(Guid id);
        public Task<Guid> Post(AddUserRequest addUserRequest);
        public Task Put(Guid id, UpdateUserRequest updateUserRequest);
    }
}
