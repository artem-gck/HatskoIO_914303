using Moq;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Realisations;
using SignatureService.DataAccess.DataBase;
using System.Data;
using Moq.Dapper;
using Dapper;
using SignatureService.DataAccess.DataBase.Exceptiions;

namespace SignatureService.Test.Repositories.User
{
    [TestFixture]
    public class GetAsync
    {
        private UserEntity _user;
        public Guid _id;

        [SetUp]
        public void Setup()
        {
            _id = Guid.NewGuid();
            _user = new UserEntity()
            {
                Id = _id
            };
        }

        [Test]
        public async Task GetAsync_Id_User()
        {
            var connectionMock = new Mock<IDbConnection>();

            connectionMock.SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<UserEntity>(It.IsAny<string>(), null, null, null, null))
                          .ReturnsAsync(_user);

            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);
            connectionProviderMock.Setup(p => p.GetDbConnection())
                                  .Returns(connectionMock.Object);

            var repository = new UserRepository(connectionProviderMock.Object);

            var result = await repository.GetAsync(_id);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(_id));
            }); 
        }

        [Test]
        public async Task GetAsync_Id_NotFoundException()
        {
            UserEntity user = default;

            var connectionMock = new Mock<IDbConnection>();

            connectionMock.SetupDapperAsync(c => c.QueryAsync<UserEntity>(It.IsAny<string>(), null, null, null, null))
                       .ReturnsAsync(new List<UserEntity>());

            var connectionProviderMock = new Mock<SqlServerConnectionProvider>(string.Empty);
            connectionProviderMock.Setup(p => p.GetDbConnection())
                                  .Returns(connectionMock.Object);

            var repository = new UserRepository(connectionProviderMock.Object);

            Assert.ThrowsAsync<NotFoundException>(() => repository.GetAsync(_id));
        }
    }
}
