using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;

namespace CompanyManagementService.DataAccess.Interfaces
{
    public interface IPositionsRepository
    {
        public Task<PositionResponce> GetAsync(Guid id);
        public Task<IEnumerable<PositionResponce>> GetAsync();
        public Task DeleteAsync(Guid id);
        public Task<Guid> PostAsync(AddPositionRequest addPositionRequest);
        public Task PutAsync(Guid id, UpdatePositionRequest updatePositionRequest);
    }
}
