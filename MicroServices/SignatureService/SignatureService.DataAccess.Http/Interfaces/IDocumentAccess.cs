using SignatureService.DataAccess.Http.Responce;

namespace SignatureService.DataAccess.Http.Interfaces
{
    public interface IDocumentAccess
    {
        public Task<HashResponce> GetHashAsync(Guid documentId, int version);
        public Task<HashResponce> GetHashAsync(Guid documentId, int version, string token);
    }
}
