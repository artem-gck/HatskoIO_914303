using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.Services.Interfaces;
using System.Security.Cryptography;

namespace SignatureService.Services.Realisations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Guid> AddUserAsync(Guid id)
        {
            using var rsa = RSA.Create();

            var publicKey = rsa.ExportRSAPublicKey();
            var privateKey = rsa.ExportRSAPrivateKey();

            var user = new UserEntity
            {
                Id = id,
                PublicKey = publicKey,
                PrivateKey = privateKey,
            };

            return await _userRepository.AddUserAsync(user);
        }
    }
}
