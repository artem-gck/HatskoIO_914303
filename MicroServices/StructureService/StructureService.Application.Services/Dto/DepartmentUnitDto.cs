namespace StructureService.Application.Services.Dto
{
    public class DepartmentUnitDto : BaseDto
    {
        public Guid UserId { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public Guid CheifUserId { get; set; }
    }
}
