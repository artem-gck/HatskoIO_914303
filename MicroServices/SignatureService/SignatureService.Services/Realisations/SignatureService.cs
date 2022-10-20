using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.Services.Interfaces;
using System.Security.Cryptography;

namespace SignatureService.Services.Realisations
{
    public class SignatureService : ISignatureService
    {
        private readonly IDocumentAccess _documentAccess;
        private readonly ISignatureRepository _signatureRepository;
        private readonly IUserRepository _userRepository;

        public SignatureService(IDocumentAccess documentAccess, ISignatureRepository signatureRepository, IUserRepository userRepository)
        {
            _documentAccess = documentAccess ?? throw new ArgumentNullException(nameof(documentAccess));
            _signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task AddAsync(Guid userId, Guid documentId, int version)
        {
            var user = await _userRepository.GetAsync(userId);
            var hash = await _documentAccess.GetHashAsync(documentId, version);

            var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(user.PublicKey, out int bytesRead);
            rsa.ImportRSAPrivateKey(user.PrivateKey, out bytesRead);

            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm(nameof(SHA256));

            var signedHash = rsaFormatter.CreateSignature(hash.Hash);

            var signatureEntity = new SignatureEntity()
            {
                DocumentId = documentId,
                Version = version,
                Hash = signedHash,
                UserId = userId
            };

            await _signatureRepository.AddAsync(signatureEntity);
        }

        public async Task<bool> CheckDocumentByUser(Guid userId, Guid documentId, int version)
        {
            var signature = await _signatureRepository.GetSignatureAync(userId, documentId, version);

            return signature is not null;
        }

        public async Task<IEnumerable<Guid>> GetUsersByDocumentIdAsync(Guid id, int version)
        {
            var signatures = await _signatureRepository.GetByDocumentIdAsync(id, version);

            return signatures.Select(sig => sig.UserId);
        }
    }
}
