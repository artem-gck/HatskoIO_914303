namespace CompanyManagementService.Services.Dto
{
    public class CheifStructureDto
    {
        public string Department { get; set; }
        public UserDto Cheif { get; set; }
        public IEnumerable<UserDto> Subordinates { get; set; }
    }
}
