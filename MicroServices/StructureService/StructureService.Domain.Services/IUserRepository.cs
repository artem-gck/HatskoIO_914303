using StructureService.Domain.Entities;

namespace StructureService.Domain.Services
{
    public interface IUserRepository
    {
        public Task<DepartmentUnitEntity> GetAsync(Guid id);
    }
}
