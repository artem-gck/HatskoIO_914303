using Microsoft.OpenApi.Any;
using Moq;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.DataAccess.Http.Interfaces;
using SignatureService.DataAccess.Http.Responce;
using SignatureService.Services.Realisations;
using System.Security.Cryptography;

namespace SignatureService.Test.Services.Signature
{
    public class CheckDocumentByUserAsync
    {
        private Guid _documentId;
        private byte[] _publicKey;
        private byte[] _privateKey;
        private byte[] _documentHash;
        private List<byte[]> _documentHashes;

        [SetUp]
        public void Setup()
        {
            using var rsa = RSA.Create();

            _documentId = Guid.NewGuid();
            _publicKey = rsa.ExportRSAPublicKey();
            _privateKey = rsa.ExportRSAPrivateKey();
            _documentHash = SHA256.HashData(GetByteArray(256));
            _documentHashes = new List<byte[]>();

            foreach (var i in Enumerable.Range(0, 4))
            {
                using var rsa1 = RSA.Create();
                var rsaFormatter1 = new RSAPKCS1SignatureFormatter(rsa1);
                rsaFormatter1.SetHashAlgorithm(nameof(SHA256));

                var signedHash1 = rsaFormatter1.CreateSignature(_documentHash);
                _documentHashes.Add(signedHash1);
            }
        }

        [Test]
        public async Task CheckDocumentByUserAsync_publicKey_true()
        {
            int buffer;

            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(_publicKey, out buffer);
            rsa.ImportRSAPrivateKey(_privateKey, out buffer);

            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            rsaFormatter.SetHashAlgorithm(nameof(SHA256));

            var signedHash = rsaFormatter.CreateSignature(_documentHash);
            _documentHashes.Add(signedHash);

            var docAccessMock = new Mock<IDocumentAccess>();
            docAccessMock.Setup(p => p.GetHashAsync(_documentId, 0, null))
                         .Returns(Task.FromResult(new HashResponce() { Hash = _documentHash }));

            var signRepositoryMock = new Mock<ISignatureRepository>();
            signRepositoryMock.Setup(p => p.GetDocumentHashes(_documentId, 0))
                              .Returns(Task.FromResult(_documentHashes.AsEnumerable()));

            var userRepositoryMock = new Mock<IUserRepository>();

            var signService = new SignService(docAccessMock.Object, signRepositoryMock.Object, userRepositoryMock.Object);

            var result = await signService.CheckDocumentByUserAsync(_documentId, 0, _publicKey);

            docAccessMock.Verify(p => p.GetHashAsync(_documentId, 0, null), Times.Once);
            signRepositoryMock.Verify(p => p.GetDocumentHashes(_documentId, 0), Times.Once);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task CheckDocumentByUserAsync_publicKey_false()
        {
            var docAccessMock = new Mock<IDocumentAccess>();
            docAccessMock.Setup(p => p.GetHashAsync(_documentId, 0, null))
                         .Returns(Task.FromResult(new HashResponce() { Hash = _documentHash }));

            var signRepositoryMock = new Mock<ISignatureRepository>();
            signRepositoryMock.Setup(p => p.GetDocumentHashes(_documentId, 0))
                              .Returns(Task.FromResult(_documentHashes.AsEnumerable()));

            var userRepositoryMock = new Mock<IUserRepository>();

            var signService = new SignService(docAccessMock.Object, signRepositoryMock.Object, userRepositoryMock.Object);

            var result = await signService.CheckDocumentByUserAsync(_documentId, 0, _publicKey);

            docAccessMock.Verify(p => p.GetHashAsync(_documentId, 0, null), Times.Once);
            signRepositoryMock.Verify(p => p.GetDocumentHashes(_documentId, 0), Times.Once);


            Assert.IsFalse(result);
        }

        [Test]
        public async Task CheckDocumentByUserAsync_publicKeyIsNUll_ArgumentNullException()
        {
            var docAccessMock = new Mock<IDocumentAccess>();
            docAccessMock.Setup(p => p.GetHashAsync(_documentId, 0, null))
                         .Returns(Task.FromResult(new HashResponce() { Hash = _documentHash }));

            var signRepositoryMock = new Mock<ISignatureRepository>();
            signRepositoryMock.Setup(p => p.GetDocumentHashes(_documentId, 0))
                              .Returns(Task.FromResult(_documentHashes.AsEnumerable()));

            var userRepositoryMock = new Mock<IUserRepository>();

            var signService = new SignService(docAccessMock.Object, signRepositoryMock.Object, userRepositoryMock.Object);
            Assert.ThrowsAsync<ArgumentNullException>(() => signService.CheckDocumentByUserAsync(_documentId, 0, null));
        }

        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(-100)]
        [TestCase(int.MinValue)]
        public async Task CheckDocumentByUserAsync_versionLessMinusOne_ArgumentOutOfRangeException(int version)
        {
            var docAccessMock = new Mock<IDocumentAccess>();
            docAccessMock.Setup(p => p.GetHashAsync(_documentId, 0, null))
                         .Returns(Task.FromResult(new HashResponce() { Hash = _documentHash }));

            var signRepositoryMock = new Mock<ISignatureRepository>();
            signRepositoryMock.Setup(p => p.GetDocumentHashes(_documentId, 0))
                              .Returns(Task.FromResult(_documentHashes.AsEnumerable()));

            var userRepositoryMock = new Mock<IUserRepository>();

            var signService = new SignService(docAccessMock.Object, signRepositoryMock.Object, userRepositoryMock.Object);
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => signService.CheckDocumentByUserAsync(_documentId, version, _publicKey));
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