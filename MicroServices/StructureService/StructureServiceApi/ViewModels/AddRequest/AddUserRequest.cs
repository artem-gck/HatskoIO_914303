using System.ComponentModel.DataAnnotations;

namespace StructureServiceApi.ViewModels.AddRequest
{
    public class AddUserRequest
    {
        [Required]
        public Guid Id { get; set; }
        public int Salary { get; set; }

        [Required]
        public string Position { get; set; }
        public Guid CheifUserId { get; set; }
    }
}
