using StructureService.Application.Services.Dto;
using StructureService.Application.Services;
using StructureService.Domain.Entities;
using StructureService.Domain.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace StructureService.Application.Realisation
{
    public class Service<TDto, TEntity> : IService<TDto> where TDto : BaseDto
                                                         where TEntity : BaseEntity
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _applicationMapper;
        private readonly ILogger<Service<TDto, TEntity>> _logger;

        public Service(IRepository<TEntity> repository, IMapper applicationMapper, ILogger<Service<TDto, TEntity>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _applicationMapper = applicationMapper ?? throw new ArgumentNullException(nameof(applicationMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> AddAsync(TDto dto)
        {
            _logger.LogDebug("Add to db dtp = {@Dto}", dto);

            return await _repository.AddAsync(_applicationMapper.Map<TEntity>(dto));
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            _logger.LogDebug("{type} deleting from db with id = {id}", typeof(TEntity), id);

            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            _logger.LogDebug("Get all {type} from db", typeof(TEntity));

            return (await _repository.GetAllAsync()).Select(us => _applicationMapper.Map<TDto>(us));
        }

        public async Task<TDto> GetAsync(Guid id)
        {
            var dto = _applicationMapper.Map<TDto>(await _repository.GetAsync(id));

            _logger.LogDebug("Get {type} from db, dto = {@Dto}", typeof(TEntity), dto);

            return dto;
        }

        public async Task<Guid> UpdateAsync(Guid id, TDto dto)
        {
            _logger.LogDebug("Update {type} in db, new dto = {@Dto}", typeof(TEntity), dto);

            return await _repository.UpdateAsync(id, _applicationMapper.Map<TEntity>(dto));
        }
    }
}
