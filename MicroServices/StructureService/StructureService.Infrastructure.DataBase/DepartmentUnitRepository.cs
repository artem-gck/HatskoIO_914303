using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StructureService.Domain.Entities;
using StructureService.Domain.Exceptions;
using StructureService.Domain.Services;
using StructureService.Infrastructure.DataBase.Context;

namespace StructureService.Infrastructure.DataBase
{
    public class DepartmentUnitRepository : IRepository<DepartmentUnitEntity>
    {
        private readonly StructureContext _structureContext;
        private readonly ILogger<DepartmentUnitRepository> _logger;

        public DepartmentUnitRepository(StructureContext structureContext, ILogger<DepartmentUnitRepository> logger)
        {
            _structureContext = structureContext ?? throw new ArgumentNullException(nameof(structureContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Guid> AddAsync(DepartmentUnitEntity entity)
        {
            entity.Department = await GetDepartment(entity.Department.Name);
            entity.Position = await GetPosition(entity.Position.Name);

            var entityDb = _structureContext.DepartmentUnits.Add(entity);

            await _structureContext.SaveChangesAsync();

            return entityDb.Entity.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var entity = await _structureContext.DepartmentUnits.FindAsync(id);

            if (entity is null)
                throw new NotFoundException<DepartmentUnitEntity>(id);

            _structureContext.DepartmentUnits.Remove(entity);

            await _structureContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<IEnumerable<DepartmentUnitEntity>> GetAllAsync()
        {
            var listOfEntities = await _structureContext.DepartmentUnits.Include(un => un.Position)
                                                                        .Include(un => un.Department)
                                                                        .ToListAsync();

            return listOfEntities;
        }

        public async Task<DepartmentUnitEntity> GetAsync(Guid id)
        {
            var entity = await _structureContext.DepartmentUnits.Include(un => un.Position)
                                                                .Include(un => un.Department)
                                                                .FirstOrDefaultAsync(un => un.Id == id);

            if (entity is null)
                throw new NotFoundException<DepartmentUnitEntity>(id);

            return entity;
        }

        public async Task<Guid> UpdateAsync(Guid id, DepartmentUnitEntity entity)
        {
            entity.Id = id;
            entity.Department = await GetDepartment(entity.Department.Name);
            entity.Position = await GetPosition(entity.Position.Name);

            _structureContext.DepartmentUnits.Update(entity);

            _structureContext.SaveChanges();

            return id;
        }

        private async Task<DepartmentEntity> GetDepartment(string name)
        {
            var entity = await _structureContext.Departments.FirstOrDefaultAsync(dep => dep.Name == name);

            if (entity is null)
                throw new NotFoundException<DepartmentEntity>(name);

            return entity;
        }

        private async Task<PositionEntity> GetPosition(string name)
        {
            var entity = await _structureContext.Positions.FirstOrDefaultAsync(pos => pos.Name == name);

            if (entity is null)
                throw new NotFoundException<DepartmentEntity>(name);

            return entity;
        }
    }
}
