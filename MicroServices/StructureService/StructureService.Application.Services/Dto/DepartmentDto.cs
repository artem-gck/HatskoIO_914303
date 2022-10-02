namespace StructureService.Application.Services.Dto
{
    public class DepartmentDto : BaseDto
    {
        public string Name { get; set; }
        public Guid CheifUserId { get; set; }
    }
}
