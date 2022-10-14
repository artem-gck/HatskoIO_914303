using CompanyManagementService.Services.Dto;

namespace CompanyManagementService.Services.Interfaces
{
    public interface IStructureService
    {
        public Task<CheifStructureDto> GetCheifStructure(Guid cheifId);
        public Task<UserDto> GetUser(Guid userId);
    }
}
