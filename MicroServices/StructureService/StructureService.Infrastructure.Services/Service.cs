using StructureService.Application.Services;
using StructureService.Domain.Entities;
using StructureService.Domain.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace StructureService.Infrastructure.Services
{
    public class Service<T> : IService<T> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;
        private readonly ILogger<Service<T>> _logger;

        public Service(IRepository<T> repository, ILogger<Service<T>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid> AddAsync(T entity)
        {
            _logger.LogDebug("Add to db dtp = {@Dto}", entity);

            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            _logger.LogDebug("{type} deleting from db with id = {id}", typeof(T), id);

            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            _logger.LogDebug("Get all {type} from db", typeof(T));

            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(int page, int count)
        {
            _logger.LogDebug("Get all {type} from db", typeof(T));

            return await _repository.GetAllAsync(page, count);
        }

        public async Task<T> GetAsync(Guid id)
        {
            var dto = await _repository.GetAsync(id);

            _logger.LogDebug("Get {type} from db, dto = {@Dto}", typeof(T), dto);

            return dto;
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            _logger.LogDebug("Update {type} in db, new dto = {@Dto}", typeof(T), entity);

            await _repository.UpdateAsync(id, entity);
        }
    }
}
