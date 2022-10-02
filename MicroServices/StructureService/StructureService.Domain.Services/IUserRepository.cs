using StructureService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureService.Domain.Services
{
    public interface IUserRepository
    {
        public Task<DepartmentUnitEntity> GetAsync(Guid id);
    }
}
