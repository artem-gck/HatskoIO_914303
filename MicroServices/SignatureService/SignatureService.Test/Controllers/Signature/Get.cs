using Microsoft.AspNetCore.Mvc;
using Moq;
using SignatureService.Services.Dto;
using SignatureService.Services.Interfaces;
using SignatureServiceApi.Controllers;

namespace SignatureService.Test.Controllers.Signature
{
    [TestFixture]
    public class Get
    {
        [Test]
        public async Task Get_ReturnsAOk_WithUserAndDocument()
        {
            // Arrange
            var mockService = new Mock<ISignService>();
            mockService.Setup(repo => repo.GetUsersByDocumentIdAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                       .Returns(Task.FromResult(It.IsAny<IEnumerable<UserPublicKey>>()));

            var controller = new SignatureController(mockService.Object);

            // Act
            var result = await controller.Get(It.IsAny<Guid>(), It.IsAny<int>());

            // Assert
            Assert.That(result, Is.InstanceOf(typeof(OkObjectResult)));
        }

        [Test]
        [TestCase(-2)]
        public async Task Get_ReturnsABadRequest_WithVersionLessMinusOne(int version)
        {
            // Arrange
            var mockService = new Mock<ISignService>();

            var controller = new SignatureController(mockService.Object);

            // Act
            var result = await controller.Get(It.IsAny<Guid>(), version);

            // Assert
            Assert.That(result, Is.InstanceOf(typeof(BadRequestObjectResult)));
        }
    }
}
