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
        private readonly IMapper _mapper;

        public StructureService(IUserInfoRepository userInfoRepository, IUserStructureRepository userStructureRepository, IMapper mapper)
        {
            _userInfoRepository = userInfoRepository ?? throw new ArgumentNullException(nameof(userInfoRepository));
            _userStructureRepository = userStructureRepository ?? throw new ArgumentNullException(nameof(userStructureRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CheifStructureDto> GetCheifStructure(Guid cheifId)
        {
            var cheif = await _userInfoRepository.Get(cheifId);
            var cheifInfo = await _userStructureRepository.Get(cheif.DepartmentId.Value, cheifId);
            var users = (await _userInfoRepository.Get()).Where(us => us.DepartmentId == cheif.DepartmentId && us.Id != cheifId);

            var listOfUsersInfo = new List<Tuple<UserResponce, DataAccess.UserEntity.UserResponce>>();

            foreach (var us in users)
            {
                var userStructure = await _userStructureRepository.Get(us.DepartmentId.Value, us.Id);
                listOfUsersInfo.Add(new (userStructure, us));
            }

            var cheifDto = _mapper.Map<UserDto>(new Tuple<UserResponce, DataAccess.UserEntity.UserResponce>(cheifInfo, cheif));
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(listOfUsersInfo);

            var cheifStructureDto = new CheifStructureDto()
            {
                Department = cheifInfo.Department,
                Cheif = cheifDto,
                Subordinates = usersDto
            };

            return cheifStructureDto;
        }
    }
}
