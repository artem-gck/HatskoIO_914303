using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;
using Microsoft.Extensions.Primitives;

namespace CompanyManagementService.DataAccess.Interfaces
{
    public interface IUserStructureAccess
    {
        public Task<UserResponce> GetAsync(Guid departmentId, Guid userId);
        public Task<UserResponce> GetAsync(Guid departmentId, Guid userId, string token);
        public Task<IEnumerable<UserResponce>> GetByDepartmentIdAsync(Guid id);
        public Task<IEnumerable<UserResponce>> GetByDepartmentIdAsync(Guid id, string token);
        public Task DeleteAsync(Guid departmentId, Guid userId);
        public Task<Guid> PostAsync(Guid departmentId, AddUserRequest userViewModel);
        public Task PutAsync(Guid departmentId, Guid userId, UpdateUserRequest userViewModel);
    }
}
