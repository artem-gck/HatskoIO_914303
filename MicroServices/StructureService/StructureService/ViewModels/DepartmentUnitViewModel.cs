using System.ComponentModel.DataAnnotations;

namespace StructureService.ViewModels
{
    public class DepartmentUnitViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Department { get; set; }

        public Guid CheifUserId { get; set; }
    }
}
