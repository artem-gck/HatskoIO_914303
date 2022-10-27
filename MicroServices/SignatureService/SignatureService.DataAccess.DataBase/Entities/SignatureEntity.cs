namespace SignatureService.DataAccess.DataBase.Entities
{
    public class SignatureEntity
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public int Version { get; set; }
        public byte[] Hash { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; } 
    }
}
