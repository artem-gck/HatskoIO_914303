using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StructureService.Dimain.Realisation.Context;
using StructureService.Domain.Entities;
using StructureService.Domain.Exceptions;
using StructureService.Domain.Services;

namespace StructureService.Dimain.Realisation
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

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var entity = await _entities.FindAsync(id);

            if (entity is null)
                throw new NotFoundException<T>(id);

            _entities.Remove(entity);

            await _structureContext.SaveChangesAsync();

            return entity.Id;
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

        public async Task<Guid> UpdateAsync(Guid id, T entity)
        {
            var entityDb = await _entities.FindAsync(id);

            if (entityDb is null)
                throw new NotFoundException<T>(id);

            entity.Id = entityDb.Id;
            
            _entities.Update(entity);

            _structureContext.SaveChanges();

            return id;
        }
    }
}
