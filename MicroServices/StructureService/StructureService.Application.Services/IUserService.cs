using StructureService.Domain.Entities;

namespace StructureService.Application.Services
{
    public interface IUserService
    {
        public Task<DepartmentUnitEntity> GetAsync(Guid id);
    }
}
