using SignatureService.Services.Interfaces;

namespace SignatureService.Services.Realisations
{
    public class SignatureService : ISignatureService
    {
        public Task<Guid> AddAsync(Guid userId, Guid documentId, int version)
        {
            throw new NotImplementedException();
        }

        public Task GetByDocumentIdAsync(Guid id, int version)
        {
            throw new NotImplementedException();
        }
    }
}
