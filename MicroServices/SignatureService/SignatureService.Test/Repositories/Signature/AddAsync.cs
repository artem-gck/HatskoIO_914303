using Moq;
using SignatureService.DataAccess.DataBase.Realisations;
using SignatureService.DataAccess.DataBase;
using System.Data;
using Moq.Dapper;
using Dapper;
using SignatureService.DataAccess.DataBase.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.Common;

namespace SignatureService.Test.Repositories.Signature
{
    public class AddAsync
    {
        private SignatureEntity _signature;

        [SetUp]
        public void Setup()
        {
            _signature = new SignatureEntity();
        }

        [Test]
        public async Task AddAsync_Signature_Task()
        {
            var connectionMock = new Mock<DbConnection>();

            connectionMock.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), null, null, null, null))
                      .ReturnsAsync(It.IsAny<int>());

            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);
            connectionProviderMock.Setup(p => p.GetDbConnection())
                                  .Returns(connectionMock.Object);

            var repository = new SignatureRepository(connectionProviderMock.Object);

            await repository.AddAsync(_signature);

            connectionProviderMock.Verify(c => c.GetDbConnection(), Times.Once);
        }

        [Test]
        public async Task AddAsync_Null_ArgumentNullException()
        {
            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);

            var repository = new SignatureRepository(connectionProviderMock.Object);

            connectionProviderMock.Verify(c => c.GetDbConnection(), Times.Never);

            Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddAsync(null));
        }
    }
}
