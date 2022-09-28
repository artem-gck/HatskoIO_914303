namespace UserLoginService.Domain.Entities
{
    
    public class UserLoginEntity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public RoleEntity Role { get; set; }
        public int? TockenId { get; set; }
        public int? UserInfoId { get; set; }
    }
}
