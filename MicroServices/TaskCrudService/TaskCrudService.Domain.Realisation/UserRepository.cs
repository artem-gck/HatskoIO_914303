using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskCrudService.Domain.Entities;
using TaskCrudService.Domain.Realisation.Context;
using TaskCrudService.Domain.Services;

namespace TaskCrudService.Domain.Realisation
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskContext _taskContext;

        public UserRepository(TaskContext taskContext)
        {
            _taskContext = taskContext ?? throw new ArgumentNullException(nameof(taskContext));
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync(Guid id)
        {
            var listOfTaskByUser = await _taskContext.Tasks.Include(t => t.Type)
                                                           .Include(t => t.Arguments)
                                                               .ThenInclude(ar => ar.ArgumentType)
                                                           .Where(t => t.OwnerUserId == id)
                                                           .ToListAsync();

            return listOfTaskByUser;
        }
    }
}
