using StructureService.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureService.Application.Services
{
    public interface IUserService
    {
        public Task<DepartmentUnitDto> GetAsync(Guid id);
    }
}
