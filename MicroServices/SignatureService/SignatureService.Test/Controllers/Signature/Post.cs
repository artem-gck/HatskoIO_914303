using Microsoft.AspNetCore.Mvc;
using Moq;
using SignatureService.Services.Interfaces;
using SignatureServiceApi.Controllers;
using Microsoft.AspNetCore.Http;

namespace SignatureService.Test.Controllers.Signature
{
    public class Post
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
        public async Task Post_ReturnsACreate_WithUserAndDocument()
        {
            // Arrange
            var mockService = new Mock<ISignService>();
            mockService.Setup(repo => repo.AddAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>()))
                       .Returns(Task.CompletedTask);
            
            var controller = new SignatureController(mockService.Object);
            controller.ControllerContext.HttpContext = _httpContext.Object;

            // Act
            var result = await controller.Post(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>());
            
            // Assert
            Assert.IsInstanceOf(typeof(CreatedResult), result);
        }

        [TestCase(-2)]
        [TestCase(-50)]
        [TestCase(-100)]
        [TestCase(int.MinValue)]
        public async Task Post_ReturnsABadRequest_WithVersionLessMinusOne(int version)
        {
            // Arrange
            var mockService = new Mock<ISignService>();

            var controller = new SignatureController(mockService.Object);
            controller.ControllerContext.HttpContext = _httpContext.Object;

            // Act
            var result = await controller.Post(It.IsAny<Guid>(), It.IsAny<Guid>(), version);

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }
    }
}
