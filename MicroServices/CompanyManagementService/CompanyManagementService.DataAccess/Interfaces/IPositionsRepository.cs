using CompanyManagementService.DataAccess.StructureEntities.AddRequest;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.DataAccess.StructureEntities.UpdateRequest;

namespace CompanyManagementService.DataAccess.Interfaces
{
    public interface IPositionsRepository
    {
        public Task<PositionResponce> Get(Guid id);
        public Task<IEnumerable<PositionResponce>> Get();
        public Task Delete(Guid id);
        public Task<Guid> Post(AddPositionRequest addPositionRequest);
        public Task Put(Guid id, UpdatePositionRequest updatePositionRequest);
    }
}
