using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskCrudService.Domain.Entities;

namespace TaskCrudService.Domain.Services
{
    public interface IUserRepository
    {
        public Task<IEnumerable<TaskEntity>> GetAllAsync(Guid id); 
    }
}
