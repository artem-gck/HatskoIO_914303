using AutoMapper;
using CompanyManagementService.Cache;
using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.Realisation;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.Services.Dto;
using CompanyManagementService.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CompanyManagementService.Services.Realisation
{
    public class StructureService : IStructureService
    {
        private readonly IUserInfoAccess _userInfoAccess;
        private readonly IUserStructureAccess _userStructureAccess;
        private readonly IPositionsAccess _positionsAccess;
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;

        public StructureService(IUserInfoAccess userInfoAccess, IUserStructureAccess userStructureAccess, IPositionsAccess positionsAccess, IDistributedCache cache, IMapper mapper)
        {
            _userInfoAccess = userInfoAccess ?? throw new ArgumentNullException(nameof(userInfoAccess));
            _userStructureAccess = userStructureAccess ?? throw new ArgumentNullException(nameof(userStructureAccess));
            _positionsAccess = positionsAccess ?? throw new ArgumentNullException(nameof(positionsAccess));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CheifStructureDto> GetCheifStructureAsync(Guid cheifId)
        {
            var cacheId = $"Cheif_{cheifId}";
            var cheifStructureDto = await _cache.GetRecordAsync<CheifStructureDto>(cacheId);

            if (cheifStructureDto is null)
            {
                var cheif = await _userInfoAccess.GetAsync(cheifId);
                var cheifInfo = await _userStructureAccess.GetAsync(cheif.DepartmentId.Value, cheifId);
                
                var users = (await _userInfoAccess.GetByDepartmentIdAsync(cheif.DepartmentId.Value))
                                                      .Where(us => us.Id != cheifId);
                var usersInfo = (await _userStructureAccess.GetByDepartmentIdAsync(cheif.DepartmentId.Value))
                                                               .Where(us => us.CheifUserId == cheifId);

                var listOfUsersInfo = usersInfo.Zip(users).ToList();

                var cheifDto = _mapper.Map<UserDto>((cheifInfo, cheif));
                var usersDto = _mapper.Map<IEnumerable<UserDto>>(listOfUsersInfo);

                cheifStructureDto = new CheifStructureDto()
                {
                    Department = cheifInfo.Department,
                    Cheif = cheifDto,
                    Subordinates = usersDto
                };

                await _cache.SetRecordAsync(cacheId, cheifStructureDto);
            }

            return cheifStructureDto;
        }

        public async Task<UserDto> GetUserAsync(Guid userId)
        {
            var cacheId = $"User_{userId}";
            var userDto = await _cache.GetRecordAsync<UserDto>(cacheId);

            if (userDto is null)
            {
                var user = await _userInfoAccess.GetAsync(userId);
                var position = await _positionsAccess.GetAsync(user.PositionId.Value);

                userDto = _mapper.Map<UserDto>((position, user));
                await _cache.SetRecordAsync(cacheId, userDto);
            }

            return userDto;
        }
    }
}
