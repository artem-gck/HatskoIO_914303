using StructureService.Application.Services.Dto;
using StructureService.Application.Services;
using StructureService.Domain.Entities;
using StructureService.Domain.Services;
using AutoMapper;

namespace StructureService.Application.Realisation
{
    public class Service<TDto, TEntity> : IService<TDto> where TDto : BaseDto
                                                         where TEntity : BaseEntity
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _applicationMapper;

        public Service(IRepository<TEntity> repository, IMapper applicationMapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _applicationMapper = applicationMapper ?? throw new ArgumentNullException(nameof(applicationMapper));
        }

        public async Task<int> AddAsync(TDto dto)
        {
            return await _repository.AddAsync(_applicationMapper.Map<TEntity>(dto));
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            return (await _repository.GetAllAsync()).Select(us => _applicationMapper.Map<TDto>(us));
        }

        public async Task<TDto> GetAsync(int id)
        {
            return _applicationMapper.Map<TDto>(await _repository.GetAsync(id));
        }

        public async Task<int> UpdateAsync(int id, TDto dto)
        {
            return await _repository.UpdateAsync(id, _applicationMapper.Map<TEntity>(dto));
        }
    }
}
