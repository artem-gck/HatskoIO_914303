namespace SignatureService.DataAccess.DataBase.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public byte[] PublicKey { get; set; }
        public byte[] PrivateKey { get; set; }
        public IEnumerable<SignatureEntity> Signatures { get; set; }
    }
}
