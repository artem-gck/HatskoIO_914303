namespace UsersService.Services.Dto
{
    /// <summary>
    /// Data transfer object for info about user
    /// </summary>
    public class UserInfoDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
    }
}
