using System.ComponentModel.DataAnnotations;

namespace StructureService.ViewModels
{
    public class DepartmentViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,20}[a-zA-Z]$")]
        public string Name { get; set; }

        [Required]
        public Guid CheifUserId { get; set; }
    }
}
