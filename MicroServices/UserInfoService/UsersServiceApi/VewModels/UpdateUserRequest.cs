using System.ComponentModel.DataAnnotations;

namespace UsersServiceApi.VewModels
{
    public class UpdateUserRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,20}[a-zA-Z]$")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,20}[a-zA-Z]$")]
        public string Surname { get; set; }

        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,30}[a-zA-Z]$")]
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? PositionId { get; set; }
    }
}
