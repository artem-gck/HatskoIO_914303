using CompanyManagementService.Services.Dto;

namespace CompanyManagementServiceApi.ViewModels
{
    public class CheifStructureResponce
    {
        public string Department { get; set; }
        public UserDto Cheif { get; set; }
        public IEnumerable<UserResponce> Subordinates { get; set; }
    }
}
