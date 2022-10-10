using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StructureService.Domain.Entities;
using StructureService.Domain.Exceptions;
using StructureService.Domain.Services;
using StructureService.Infrastructure.DataBase.Context;

namespace StructureService.Infrastructure.DataBase
{
    public class UserRepository : IUserRepository
    {
        private readonly StructureContext _structureContext;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(StructureContext structureContext, ILogger<UserRepository> logger)
        {
            _structureContext = structureContext ?? throw new ArgumentNullException(nameof(structureContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Guid> AddAsync(User entity)
        {
            entity.Department = await GetDepartment(entity.Department.Name);
            entity.Position = await GetPosition(entity.Position.Name);

            var entityDb = _structureContext.DepartmentUnits.Add(entity);

            await _structureContext.SaveChangesAsync();

            return entityDb.Entity.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _structureContext.DepartmentUnits.FindAsync(id);

            if (entity is null)
                throw new UserNotFoundException(id);

            _structureContext.DepartmentUnits.Remove(entity);

            await _structureContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var listOfEntities = await _structureContext.DepartmentUnits.Include(un => un.Position)
                                                                        .Include(un => un.Department)
                                                                        .ToListAsync();

            return listOfEntities;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var entity = await _structureContext.DepartmentUnits.Include(un => un.Position)
                                                                .Include(un => un.Department)
                                                                .FirstOrDefaultAsync(un => un.Id == id);

            if (entity is null)
                throw new UserNotFoundException(id);

            return entity;
        }

        public async Task UpdateAsync(Guid id, User entity)
        {
            entity.Id = id;
            entity.Department = await GetDepartment(entity.Department.Name);
            entity.Position = await GetPosition(entity.Position.Name);

            _structureContext.DepartmentUnits.Update(entity);

            await _structureContext.SaveChangesAsync();
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
