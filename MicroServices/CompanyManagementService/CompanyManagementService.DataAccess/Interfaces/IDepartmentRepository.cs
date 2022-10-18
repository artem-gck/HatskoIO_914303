using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;

namespace CompanyManagementService.DataAccess.Interfaces
{
    public interface IDepartmentRepository
    {
        public Task<DepartmentResponce> GetAsync(Guid id);
        public Task<IEnumerable<DepartmentResponce>> GetAsync();
        public Task DeleteAsync(Guid id);
        public Task<Guid> PostAsync(AddDepartmentRequest addDepartmentRequest);
        public Task PutAsync(Guid id, UpdateDepartmentRequest updateDepartmentRequest);
    }
}
