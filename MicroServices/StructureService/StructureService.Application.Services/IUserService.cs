using StructureService.Application.Services.Dto;

namespace StructureService.Application.Services
{
    public interface IUserService
    {
        public Task<DepartmentUnitDto> GetAsync(Guid id);
    }
}
