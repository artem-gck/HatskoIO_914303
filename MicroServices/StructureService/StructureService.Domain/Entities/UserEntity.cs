using System.ComponentModel.DataAnnotations.Schema;

namespace StructureService.Domain.Entities
{
    public class UserEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public int Salary { get; set; }
        public PositionEntity Position { get; set; }
        public DepartmentEntity Department { get; set; }
    }
}