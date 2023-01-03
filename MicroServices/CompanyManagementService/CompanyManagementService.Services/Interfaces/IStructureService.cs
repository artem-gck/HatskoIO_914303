using CompanyManagementService.Services.Dto;

namespace CompanyManagementService.Services.Interfaces
{
    public interface IStructureService
    {
        public Task<CheifStructureDto> GetCheifStructureAsync(Guid cheifId);
        public Task<CheifStructureDto> GetCheifStructureAsync(Guid cheifId, string token);
        public Task<UserDto> GetUserAsync(Guid userId);
        public Task<UserDto> GetUserAsync(Guid userId, string token);
        public Task<IEnumerable<UserDto>> GetUsersByDepartmentAsync(Guid departmentId);
        public Task<IEnumerable<UserDto>> GetUsersByDepartmentAsync(Guid departmentId, string token);
    }
}
