namespace UserLoginService.Domain.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<UserLoginEntity>? Users { get; set; }
    }
}
