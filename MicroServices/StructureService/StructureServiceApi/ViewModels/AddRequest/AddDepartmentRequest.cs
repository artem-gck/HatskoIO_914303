using System.ComponentModel.DataAnnotations;

namespace StructureServiceApi.ViewModels.AddRequest
{
    public class AddDepartmentRequest
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,20}[a-zA-Z]$")]
        public string Name { get; set; }
        public Guid? CheifUserId { get; set; }
    }
}
