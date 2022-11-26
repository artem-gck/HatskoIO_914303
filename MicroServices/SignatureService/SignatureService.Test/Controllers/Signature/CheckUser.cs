using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SignatureService.Services.Interfaces;
using SignatureServiceApi.Controllers;
using SignatureServiceApi.ViewModels;

namespace SignatureService.Test.Controllers.Signature
{
    [TestFixture]
    public class CheckUser
    {
        private Mock<HttpContext> _httpContext;

        [SetUp]
        public void Setup()
        {
            var request = new Mock<HttpRequest>();
            request.SetupGet(x => x.Headers)
                   .Returns(new HeaderDictionary
                   {
                       { "Authorization", It.IsAny<string>() }
                   });

            _httpContext = new Mock<HttpContext>();
            _httpContext.SetupGet(x => x.Request)
                        .Returns(request.Object);
        }

        [Test]
        public async Task CheckUser_ReturnsAOk_WithUserKeyAndDocument()
        {
            // Arrange
            var mockService = new Mock<ISignService>();
            mockService.Setup(repo => repo.CheckDocumentByUserAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(true));

            var controller = new SignatureController(mockService.Object);
            controller.ControllerContext.HttpContext = _httpContext.Object;

            var request = new CheckPublicKeyRequest()
            {
                Key = It.IsAny<byte[]>()
            };

            // Act
            var result = await controller.Post(request, It.IsAny<Guid>(), It.IsAny<int>());

            // Assert
            Assert.That(result, Is.InstanceOf(typeof(OkResult)));
        }

        [Test]
        public async Task CheckUser_ReturnsANotFound_WithUserKeyAndDocument()
        {
            // Arrange
            var mockService = new Mock<ISignService>();
            mockService.Setup(repo => repo.CheckDocumentByUserAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                       .Returns(Task.FromResult(false));

            var controller = new SignatureController(mockService.Object);
            controller.ControllerContext.HttpContext = _httpContext.Object;

            var request = new CheckPublicKeyRequest()
            {
                Key = It.IsAny<byte[]>()
            };

            // Act
            var result = await controller.Post(request, It.IsAny<Guid>(), It.IsAny<int>());

            // Assert
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public async Task CheckUser_ReturnsABadRequest_ModelStateError()
        {
            // Arrange
            var mockService = new Mock<ISignService>();

            var controller = new SignatureController(mockService.Object);
            controller.ControllerContext.HttpContext = _httpContext.Object;
            controller.ModelState.AddModelError("Key", "Required");

            var request = new CheckPublicKeyRequest()
            {
                Key = It.IsAny<byte[]>()
            };

            // Act
            var result = await controller.Post(request, It.IsAny<Guid>(), It.IsAny<int>());

            // Assert
            Assert.That(result, Is.InstanceOf(typeof(BadRequestObjectResult)));
        }

        [Test]
        [TestCase(-2)]
        public async Task CheckUser_ReturnsABadRequest_WithVersionLessMinusOne(int version)
        {
            // Arrange
            var mockService = new Mock<ISignService>();

            var controller = new SignatureController(mockService.Object);
            controller.ControllerContext.HttpContext = _httpContext.Object;

            // Act
            var result = await controller.Post(It.IsAny<CheckPublicKeyRequest>(), It.IsAny<Guid>(), version);

            // Assert
            Assert.That(result, Is.InstanceOf(typeof(BadRequestObjectResult)));
        }
    }
}
