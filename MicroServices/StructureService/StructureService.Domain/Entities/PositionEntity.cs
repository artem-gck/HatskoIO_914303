namespace StructureService.Domain.Entities
{
    public class PositionEntity : BaseEntity
    {
        public string Name { get; set; }
        public List<User>? DepartmentUnits { get; set; }
    }
}
