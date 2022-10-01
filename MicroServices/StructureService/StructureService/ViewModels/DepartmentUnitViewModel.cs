using System.ComponentModel.DataAnnotations;

namespace StructureService.ViewModels
{
    public class DepartmentUnitViewModel
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
