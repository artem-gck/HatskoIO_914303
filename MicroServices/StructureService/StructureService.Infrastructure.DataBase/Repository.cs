using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StructureService.Domain.Entities;
using StructureService.Domain.Exceptions;
using StructureService.Domain.Services;
using StructureService.Infrastructure.DataBase.Context;

namespace StructureService.Infrastructure.DataBase
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly StructureContext _structureContext;
        private readonly ILogger<Repository<T>> _logger;
        private DbSet<T> _entities;

        public Repository(StructureContext structureContext, ILogger<Repository<T>> logger)
        {
            _structureContext = structureContext ?? throw new ArgumentNullException(nameof(structureContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _entities = structureContext.Set<T>();
        }

        public async Task<Guid> AddAsync(T entity)
        {
            var entityDb = _entities.Add(entity);

            await _structureContext.SaveChangesAsync();

            return entityDb.Entity.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _entities.FindAsync(id);

            if (entity is null)
                throw new NotFoundException<T>(id);

            _entities.Remove(entity);

            await _structureContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var listOfEntities = await _entities.ToListAsync();

            return listOfEntities;
        }

        public async Task<T> GetAsync(Guid id)
        {
            var entity = await _entities.FindAsync(id);

            if (entity is null)
                throw new NotFoundException<T>(id);

            return entity;
        }

        public async Task UpdateAsync(Guid id, T entity)
        {
            entity.Id = id;
            
            _entities.Update(entity);

            await _structureContext.SaveChangesAsync();
        }
    }
}
