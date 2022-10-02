using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskCrudService.Application.Services.Dto;

namespace TaskCrudService.Application.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<TaskDto>> GetAllAsync(Guid id);
    }
}
