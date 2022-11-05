﻿using NotificationService.DataAccess.DataBase.Entity;

namespace NotificationService.DataAccess.DataBase.Interfaces
{
    public interface ITaskRepository
    {
        public Task<IEnumerable<IGrouping<Guid, TaskEntity>>> GetAsync();
        public Task<TaskEntity> GetAsync(Guid id);
        public Task<Guid> AddAsync(TaskEntity entity);
    }
}
