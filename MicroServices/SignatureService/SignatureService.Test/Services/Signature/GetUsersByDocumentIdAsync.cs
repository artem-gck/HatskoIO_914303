using Moq;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.Services.Realisations;
using System.Security.Cryptography;

namespace SignatureService.Test.Services.Signature
{
    [TestFixture]
    public class GetUsersByDocumentIdAsync
    {
        private Guid _documentId;
        private List<SignatureEntity> _usersPublicKeys;

        [SetUp]
        public void Setup()
        {
            _documentId = Guid.NewGuid();
            _usersPublicKeys = new List<SignatureEntity>();

            foreach (var i in Enumerable.Range(1, 5))
            {
                using var rsa = RSA.Create();

                _usersPublicKeys.Add(new SignatureEntity()
                {
                    Id = Guid.NewGuid(),
                    User = new UserEntity()
                    {
                        PublicKey = rsa.ExportRSAPublicKey()
                    }
                });
            }
        }

        [Test]
        public async Task GetUsersByDocumentIdAsync_documentId_publicKeys()
        {
            var docAccessMock = new Mock<IDocumentAccess>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var signRepositoryMock = new Mock<ISignatureRepository>();
            signRepositoryMock.Setup(p => p.GetByDocumentIdAsync(It.IsAny<Guid>(), 0))
                              .Returns(Task.FromResult(_usersPublicKeys.AsEnumerable()));

            var signService = new SignService(docAccessMock.Object, signRepositoryMock.Object, userRepositoryMock.Object);

            var result = await signService.GetUsersByDocumentIdAsync(It.IsAny<Guid>(), 0);

            signRepositoryMock.Verify(p => p.GetByDocumentIdAsync(It.IsAny<Guid>(), 0), Times.Once);

            Assert.That(5, Is.EqualTo(result.Count()));
        }

        [Test]
        public async Task GetUsersByDocumentIdAsync_documentId_NoUsers()
        {
            var docAccessMock = new Mock<IDocumentAccess>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var signRepositoryMock = new Mock<ISignatureRepository>();
            signRepositoryMock.Setup(p => p.GetByDocumentIdAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                              .Returns(Task.FromResult(new List<SignatureEntity>().AsEnumerable()));

            var signService = new SignService(docAccessMock.Object, signRepositoryMock.Object, userRepositoryMock.Object);

            var result = await signService.GetUsersByDocumentIdAsync(It.IsAny<Guid>(), It.IsAny<int>());

            signRepositoryMock.Verify(p => p.GetByDocumentIdAsync(It.IsAny<Guid>(), It.IsAny<int>()), Times.Once);

            Assert.That(0, Is.EqualTo(result.Count()));
        }

        [Test]
        [TestCase(-2)]
        public async Task GetUsersByDocumentIdAsync_versionLessMinusOne_ArgumentOutOfRangeException(int version)
        {
            var docAccessMock = new Mock<IDocumentAccess>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var signRepositoryMock = new Mock<ISignatureRepository>();

            var signService = new SignService(docAccessMock.Object, signRepositoryMock.Object, userRepositoryMock.Object);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => signService.GetUsersByDocumentIdAsync(It.IsAny<Guid>(), version));
        }
    }
}
