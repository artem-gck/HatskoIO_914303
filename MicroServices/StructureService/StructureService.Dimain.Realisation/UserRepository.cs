﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StructureService.Dimain.Realisation.Context;
using StructureService.Domain.Entities;
using StructureService.Domain.Exceptions;
using StructureService.Domain.Services;

namespace StructureService.Dimain.Realisation
{
    public class UserRepository : IUserRepository
    {
        private readonly StructureContext _structureContext;

        public UserRepository(StructureContext structureContext)
        {
            _structureContext = structureContext ?? throw new ArgumentNullException(nameof(structureContext));
        }

        public async Task<DepartmentUnitEntity> GetAsync(Guid id)
        {
            var entity = await _structureContext.DepartmentUnits.Include(un => un.Position)
                                                                .Include(un => un.Department)
                                                                .FirstOrDefaultAsync(un => un.UserId == id);

            if (entity is null)
                throw new NotFoundException<DepartmentUnitEntity>(id);

            return entity;
        }
    }
}
