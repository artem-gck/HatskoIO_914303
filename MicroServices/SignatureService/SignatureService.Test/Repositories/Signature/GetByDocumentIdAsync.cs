using Moq;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Realisations;
using SignatureService.DataAccess.DataBase;
using System.Data;
using Moq.Dapper;
using Dapper;
using System.Data.Common;
using SignatureService.DataAccess.DataBase.Exceptiions;

namespace SignatureService.Test.Repositories.Signature
{
    [TestFixture]
    public class GetByDocumentIdAsync
    {
        private List<SignatureEntity> _signatures;

        [SetUp]
        public void Setup()
        {
            _signatures = new List<SignatureEntity>() { new SignatureEntity() { Hash = new byte[] { 1, 1, 1 } } };
        }

        [Test]
        public async Task GetByDocumentIdAsync_DocumentIdWithVersion_Array()
        {
            var connectionMock = new Mock<DbConnection>();

            connectionMock.SetupDapperAsync(c => c.QueryAsync<SignatureEntity>(It.IsAny<string>(), null, null, null, null))
                          .ReturnsAsync(_signatures.AsEnumerable());

            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);
            connectionProviderMock.Setup(p => p.GetDbConnection())
                                  .Returns(connectionMock.Object);

            var repository = new SignatureRepository(connectionProviderMock.Object);

            var result = await repository.GetByDocumentIdAsync(It.IsAny<Guid>(), It.IsAny<int>());

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(_signatures, Has.Count.EqualTo(result.Count()));
            });
        }

        [Test]
        public async Task GetByDocumentIdAsync_NoUsersInDb_NotFoundException()
        {
            var connectionMock = new Mock<DbConnection>();

            connectionMock.SetupDapperAsync(c => c.QueryAsync<SignatureEntity>(It.IsAny<string>(), null, null, null, null))
                          .ReturnsAsync(new List<SignatureEntity>());

            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);
            connectionProviderMock.Setup(p => p.GetDbConnection())
                                  .Returns(connectionMock.Object);

            var repository = new SignatureRepository(connectionProviderMock.Object);

            Assert.ThrowsAsync<NotFoundException>(() => repository.GetByDocumentIdAsync(It.IsAny<Guid>(), It.IsAny<int>()));
        }

        [Test]
        [TestCase(-2)]
        public async Task GetByDocumentIdAsync_VersionLessMinusOne_ArgumentOutOfRangeException(int version)
        {
            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);

            var repository = new SignatureRepository(connectionProviderMock.Object);

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repository.GetByDocumentIdAsync(It.IsAny<Guid>(), version));
        }
    }
}
