using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;

namespace CompanyManagementService.DataAccess.Interfaces
{
    public interface IDepartmentRepository
    {
        public Task<DepartmentResponce> Get(Guid id);
        public Task<IEnumerable<DepartmentResponce>> Get();
        public Task Delete(Guid id);
        public Task<Guid> Post(AddDepartmentRequest addDepartmentRequest);
        public Task Put(Guid id, UpdateDepartmentRequest updateDepartmentRequest);
    }
}
