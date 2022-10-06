using System.ComponentModel.DataAnnotations;

namespace StructureServiceApi.ViewModels.AddRequest
{
    public class AddDepartmentUnitRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Department { get; set; }

        public Guid CheifUserId { get; set; }
    }
}
