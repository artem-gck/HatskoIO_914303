using Dapper;
using Moq;
using Moq.Dapper;
using SignatureService.DataAccess.DataBase;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Exceptiions;
using SignatureService.DataAccess.DataBase.Realisations;
using System.Data;

namespace SignatureService.Test.Repositories.Signature
{
    public class GetDocumentHashes
    {
        private List<SignatureEntity> _signatures;

        [SetUp]
        public void Setup()
        {
            _signatures = new List<SignatureEntity>() { new SignatureEntity() { Hash = new byte[] { 1, 1, 1 } } };
        }

        [Test]
        public async Task GetDocumentHashes_DocumentIdWithVersion_Array()
        {
            var connectionMock = new Mock<IDbConnection>();

            connectionMock.SetupDapperAsync(c => c.QueryAsync<SignatureEntity>(It.IsAny<string>(), null, null, null, null))
                          .ReturnsAsync(_signatures.AsEnumerable());

            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);
            connectionProviderMock.Setup(p => p.GetDbConnection())
                                  .Returns(connectionMock.Object);

            var repository = new SignatureRepository(connectionProviderMock.Object);

            var result = await repository.GetDocumentHashes(It.IsAny<Guid>(), It.IsAny<int>());

            Assert.NotNull(result);
        }

        [Test]
        public async Task GetDocumentHashes_HttpClientReturnNull_ArgumentNullException()
        {
            var connectionMock = new Mock<IDbConnection>();

            connectionMock.SetupDapperAsync(c => c.QueryAsync<SignatureEntity>(It.IsAny<string>(), null, null, null, null))
                          .ReturnsAsync(new List<SignatureEntity>());

            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);
            connectionProviderMock.Setup(p => p.GetDbConnection())
                                  .Returns(connectionMock.Object);

            var repository = new SignatureRepository(connectionProviderMock.Object);

            Assert.ThrowsAsync<NotFoundException>(() => repository.GetDocumentHashes(It.IsAny<Guid>(), It.IsAny<int>()));
        }

        [TestCase(-2)]
        [TestCase(-50)]
        [TestCase(-100)]
        [TestCase(int.MinValue)]
        public async Task GetDocumentHashes_VersionLessMinusOne_ArgumentNullException(int version)
        {
            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);

            var repository = new SignatureRepository(connectionProviderMock.Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repository.GetDocumentHashes(It.IsAny<Guid>(), version));
        }
    }
}
