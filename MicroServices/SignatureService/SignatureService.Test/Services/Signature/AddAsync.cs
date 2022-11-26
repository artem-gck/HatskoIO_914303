using Microsoft.OpenApi.Any;
using Moq;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.DataAccess.Http.Responce;
using SignatureService.Services.Realisations;
using System.Security.Cryptography;

namespace SignatureService.Test.Services.Signature
{
    [TestFixture]
    public class AddAsync
    {
        private UserEntity _userEntity;
        private HashResponce _hashResponce;

        [SetUp]
        public void Setup()
        {
            using var rsa = RSA.Create();

            _userEntity = new UserEntity()
            {
                PublicKey = rsa.ExportRSAPublicKey(),
                PrivateKey = rsa.ExportRSAPrivateKey(),
            };
            _hashResponce = new HashResponce()
            {
                Hash = SHA256.HashData(GetByteArray(256))
            };
        }

        [Test]
        public async Task AddAsync_documentId_TimesOfInvocation()
        {
            var docAccessMock = new Mock<IDocumentAccess>();
            docAccessMock.Setup(p => p.GetHashAsync(It.IsAny<Guid>(), 0, null))
                         .Returns(Task.FromResult(_hashResponce));

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(p => p.GetAsync(It.IsAny<Guid>()))
                              .Returns(Task.FromResult(_userEntity));

            var signRepositoryMock = new Mock<ISignatureRepository>();
            signRepositoryMock.Setup(p => p.AddAsync(It.IsAny<SignatureEntity>()))
                              .Returns(Task.CompletedTask);

            var signService = new SignService(docAccessMock.Object, signRepositoryMock.Object, userRepositoryMock.Object);

            await signService.AddAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), 0, null);

            docAccessMock.Verify(p => p.GetHashAsync(It.IsAny<Guid>(), 0, null), Times.Once);
            userRepositoryMock.Verify(p => p.GetAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        [TestCase(-2)]
        public async Task GetUsersByDocumentIdAsync_versionLessMinusOne_ArgumentOutOfRangeException(int version)
        {
            var docAccessMock = new Mock<IDocumentAccess>();
            docAccessMock.Setup(p => p.GetHashAsync(It.IsAny<Guid>(), It.IsAny<int>(), null))
                         .Returns(Task.FromResult(_hashResponce));

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(p => p.GetAsync(It.IsAny<Guid>()))
                              .Returns(Task.FromResult(_userEntity));

            var signRepositoryMock = new Mock<ISignatureRepository>();
            signRepositoryMock.Setup(p => p.AddAsync(It.IsAny<SignatureEntity>()))
                              .Returns(Task.CompletedTask);

            var signService = new SignService(docAccessMock.Object, signRepositoryMock.Object, userRepositoryMock.Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => signService.AddAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), version, null));
        }

        private byte[] GetByteArray(int size)
        {
            Random rnd = new Random();
            byte[] b = new byte[size];
            rnd.NextBytes(b);
            return b;
        }
    }
}
