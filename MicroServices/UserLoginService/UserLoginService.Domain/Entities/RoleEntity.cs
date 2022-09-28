namespace UserLoginService.Domain.Entities
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public List<UserLoginEntity>? Users { get; set; }
    }
}
