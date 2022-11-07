using Moq;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Interfaces;
using SignatureService.Services.Realisations;
using System.Security.Cryptography;

namespace SignatureService.Test.Services.Users
{
    public class AddUserAsync
    {
        private Guid _userId;

        [SetUp]
        public void Setup()
        {
            _userId = Guid.NewGuid();
        }

        [Test]
        public async Task AddUserAsync_userId_Id()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(p => p.AddAsync(It.IsAny<UserEntity>()))
                              .Returns(Task.FromResult(_userId));

            var userService = new UserService(userRepositoryMock.Object);

            var result = await userService.AddUserAsync(_userId);

            userRepositoryMock.Verify(p => p.AddAsync(It.IsAny<UserEntity>()), Times.Once);

            Assert.AreEqual(result, _userId);
        }
    }
}
