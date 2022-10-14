using AutoMapper;
using CompanyManagementService.DataAccess.Interfaces;
using CompanyManagementService.DataAccess.StructureEntities.Responce;
using CompanyManagementService.Services.Dto;
using CompanyManagementService.Services.Interfaces;

namespace CompanyManagementService.Services.Realisation
{
    public class StructureService : IStructureService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IUserStructureRepository _userStructureRepository;
        private readonly IPositionsRepository _positionsRepository;
        private readonly IMapper _mapper;

        public StructureService(IUserInfoRepository userInfoRepository, IUserStructureRepository userStructureRepository, IPositionsRepository positionsRepository, IMapper mapper)
        {
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
            _userStructureRepository = userStructureRepository ?? throw new ArgumentNullException(nameof(userStructureRepository));
            _positionsRepository = positionsRepository ?? throw new ArgumentNullException(nameof(positionsRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CheifStructureDto> GetCheifStructure(Guid cheifId)
        {
            var cheif = await _userInfoRepository.Get(cheifId);
            var cheifInfo = await _userStructureRepository.Get(cheif.DepartmentId.Value, cheifId);
            var users = (await _userInfoRepository.GetByDepartmentId(cheif.DepartmentId.Value)).Where(us => us.Id != cheifId);
            var usersInfo = (await _userStructureRepository.GetByDepartmentId(cheif.DepartmentId.Value)).Where(us => us.Id != cheifId);

            var listOfUsersInfo = usersInfo.Zip(users).ToList();

            var cheifDto = _mapper.Map<UserDto>((cheifInfo, cheif));
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(listOfUsersInfo);

            var cheifStructureDto = new CheifStructureDto()
            {
                Department = cheifInfo.Department,
                Cheif = cheifDto,
                Subordinates = usersDto
            };

            return cheifStructureDto;
        }

        public async Task<UserDto> GetUser(Guid userId)
        {
            var user = await _userInfoRepository.Get(userId);
            var position = await _positionsRepository.Get(user.PositionId.Value);

            var userDto = _mapper.Map<UserDto>((position, user));

            return userDto;
        }
    }
}
