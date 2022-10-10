namespace StructureService.Domain.Entities
{
    public class PositionEntity : BaseEntity
    {
        public string Name { get; set; }
        public List<UserEntity>? DepartmentUnits { get; set; }
    }
}
