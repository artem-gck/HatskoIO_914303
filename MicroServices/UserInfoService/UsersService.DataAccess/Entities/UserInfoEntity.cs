namespace UsersService.DataAccess.Entities
{
    /// <summary>
    /// Entity for user info.
    /// </summary>
    public class UserInfoEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
        public int UserId { get; set; }
    }
}
