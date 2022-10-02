namespace StructureService.Domain.Entities
{
    public class DepartmentEntity : BaseEntity
    {
        public string Name { get; set; }
        public Guid CheifUserId { get; set; }
        public List<DepartmentUnitEntity>? DepartmentUnits { get; set; }
    }
}
