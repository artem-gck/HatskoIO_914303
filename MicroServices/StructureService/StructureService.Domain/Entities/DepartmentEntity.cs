namespace StructureService.Domain.Entities
{
    public class DepartmentEntity : BaseEntity
    {
        public string Name { get; set; }
        public int CheifUserId { get; set; }
        public List<DepartmentUnitEntity> departmentUnits { get; set; }
    }
}
