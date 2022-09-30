using System.ComponentModel.DataAnnotations;

namespace UsersService.VewModels
{
    public class UserInfoViewModel
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,20}[a-zA-Z]$")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,20}[a-zA-Z]$")]
        public string Surname { get; set; }

        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]{1,30}[a-zA-Z]$")]
        public string? Patronymic { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
