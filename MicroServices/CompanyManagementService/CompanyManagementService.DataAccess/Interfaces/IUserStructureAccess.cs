using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;

namespace CompanyManagementService.DataAccess.Interfaces
{
    public interface IUserStructureAccess
    {
        public Task<UserResponce> GetAsync(Guid departmentId, Guid userId);
        public Task<IEnumerable<UserResponce>> GetByDepartmentIdAsync(Guid id);
        public Task DeleteAsync(Guid departmentId, Guid userId);
        public Task<Guid> PostAsync(Guid departmentId, AddUserRequest userViewModel);
        public Task PutAsync(Guid departmentId, Guid userId, UpdateUserRequest userViewModel);
    }
}
