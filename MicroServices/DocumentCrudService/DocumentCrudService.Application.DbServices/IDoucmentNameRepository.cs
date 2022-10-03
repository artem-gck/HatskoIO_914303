﻿using DocumentCrudService.Domain.Entities;

namespace DocumentCrudService.Application.DbServices
{
    public interface IDoucmentNameRepository
    {
        public Task<IEnumerable<DocumentNameEntity>> GetAllAsync();
        public Task<DocumentNameEntity> GetAsync(string id);
        public Task<byte[]> GetByNameAsync(string fileName, int version = -1);
    }
}
