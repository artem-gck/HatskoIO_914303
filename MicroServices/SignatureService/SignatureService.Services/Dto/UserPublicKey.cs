namespace SignatureService.Services.Dto
{
    public class UserPublicKey
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public byte[] PublicKey { get; set; }
    }
}
