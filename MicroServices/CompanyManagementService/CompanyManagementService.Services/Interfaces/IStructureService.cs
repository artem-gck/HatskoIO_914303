using CompanyManagementService.Services.Dto;

namespace CompanyManagementService.Services.Interfaces
{
    public interface IStructureService
    {
        public Task<CheifStructureDto> GetCheifStructure(Guid cheifId);
    }
}
