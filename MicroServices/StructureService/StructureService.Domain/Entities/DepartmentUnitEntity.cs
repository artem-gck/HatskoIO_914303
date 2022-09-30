namespace StructureService.Domain.Entities
{
    public class DepartmentUnitEntity : BaseEntity
    {
        public int UserId { get; set; }
        public PositionEntity Position { get; set; }
        public DepartmentEntity Department { get; set; }
    }
}