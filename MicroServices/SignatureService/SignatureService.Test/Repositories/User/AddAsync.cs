using Moq;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Realisations;
using SignatureService.DataAccess.DataBase;
using System.Data.Common;
using Moq.Dapper;
using Dapper;

namespace SignatureService.Test.Repositories.User
{
    public class AddAsync
    {
        private UserEntity _user;
        private Guid _id;

        [SetUp]
        public void Setup()
        {
            _id = Guid.NewGuid();
            _user = new UserEntity()
            {
                Id = _id,
            };
        }

        [Test]
        public async Task AddAsync_User_Id()
        {
            var connectionMock = new Mock<DbConnection>();

            connectionMock.SetupDapperAsync(c => c.ExecuteAsync(It.IsAny<string>(), null, null, null, null))
                          .ReturnsAsync(It.IsAny<int>());

            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);
            connectionProviderMock.Setup(p => p.GetDbConnection())
                                  .Returns(connectionMock.Object);

            var repository = new UserRepository(connectionProviderMock.Object);

            var result = await repository.AddAsync(_user);

            connectionProviderMock.Verify(c => c.GetDbConnection(), Times.Once);

            Assert.AreEqual(_id, result);
        }

        [Test]
        public async Task AddAsync_Null_ArgumentNuuException()
        {
            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);

            var repository = new UserRepository(connectionProviderMock.Object);

            Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddAsync(null));
        }
    }
}
