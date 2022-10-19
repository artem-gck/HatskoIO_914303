using CompanyManagementService.Services.Dto;

namespace CompanyManagementService.Services.Interfaces
{
    public interface IStructureService
    {
        public Task<CheifStructureDto> GetCheifStructureAsync(Guid cheifId);
        public Task<UserDto> GetUserAsync(Guid userId);
    }
}
