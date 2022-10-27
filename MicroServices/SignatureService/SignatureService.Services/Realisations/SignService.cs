using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.DataAccess.Http.Responce;
using SignatureService.Services.Dto;
using SignatureService.Services.Interfaces;
using System.Security.Cryptography;

namespace SignatureService.Services.Realisations
{
    public class SignService : ISignService
    {
        private readonly IDocumentAccess _documentAccess;
        private readonly ISignatureRepository _signatureRepository;
        private readonly IUserRepository _userRepository;

        public SignService(IDocumentAccess documentAccess, ISignatureRepository signatureRepository, IUserRepository userRepository)
        {
            _documentAccess = documentAccess ?? throw new ArgumentNullException(nameof(documentAccess));
            _signatureRepository = signatureRepository ?? throw new ArgumentNullException(nameof(signatureRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task AddAsync(Guid userId, Guid documentId, int version)
        {
            var user = await _userRepository.GetAsync(userId);
            var hash = await _documentAccess.GetHashAsync(documentId, version);

            using var rsa = RSA.Create();
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

        public async Task<bool> CheckDocumentByUserAsync(Guid documentId, int version, byte[] publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(publicKey, out int bytesRead);

            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm(nameof(SHA256));

            var signedHashes = await _signatureRepository.GetDocumentHashes(documentId, version);
            var hash = await _documentAccess.GetHashAsync(documentId, version);

            foreach (var signedHash in signedHashes)
                if (rsaDeformatter.VerifySignature(hash.Hash, signedHash))
                    return true;

            return false;
        }

        public async Task<IEnumerable<UserPublicKey>> GetUsersByDocumentIdAsync(Guid documentId, int version)
        {
            var signatures = await _signatureRepository.GetByDocumentIdAsync(documentId, version);

            return signatures.Select(sig => new UserPublicKey
            {
                Id = sig.Id,    
                PublicKey = sig.User.PublicKey
            });      
        }
    }
}