using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;

namespace CompanyManagementService.DataAccess.Interfaces
{
    public interface IUserStructureRepository
    {
        public Task<UserResponce> Get(Guid departmentId, Guid userId);
        public Task<IEnumerable<UserResponce>> GetByDepartmentId(Guid id);
        public Task Delete(Guid departmentId, Guid userId);
        public Task<Guid> Post(Guid departmentId, AddUserRequest userViewModel);
        public Task Put(Guid departmentId, Guid userId, UpdateUserRequest userViewModel);
    }
}
