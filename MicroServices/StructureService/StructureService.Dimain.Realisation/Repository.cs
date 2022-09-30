using Microsoft.EntityFrameworkCore;
using StructureService.Dimain.Realisation.Context;
using StructureService.Domain.Entities;
using StructureService.Domain.Exceptions;
using StructureService.Domain.Services;

namespace StructureService.Dimain.Realisation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly StructureContext _structureContext;
        private DbSet<T> _entities;

        public Repository(StructureContext structureContext)
        {
            _structureContext = structureContext ?? throw new ArgumentNullException(nameof(structureContext));
            _entities = structureContext.Set<T>();
        }

        public async Task<int> AddAsync(T entity)
        {
            var entityDb = _entities.Add(entity);

            await _structureContext.SaveChangesAsync();

            return entityDb.Entity.Id;
        }

        public async Task<int> DeleteAsync(int id)
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

        public async Task<T> GetAsync(int id)
        {
            var entity = await _entities.FindAsync(id);

            if (entity is null)
                throw new NotFoundException<T>(id);

            return entity;
        }

        public async Task<int> UpdateAsync(int id, T entity)
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
