namespace DocumentCrudService.Tests.Controllers
{
    public class Documents
    {
        private Mock<ICommandDispatcher> _mockCommandDispatcher;
        private Mock<IQueryDispatcher> _mockQueryDispatcher;

        [SetUp]
        public void Setup()
        {
            IResult resultIdDto = new IdDto() { Id = It.IsAny<Guid>() };

            IList<IResult> resultHashDto = new List<IResult>() { new HashDto() };
            IList<IResult> resultDocumentExistDto = new List<IResult>() { new DocumentExistDto() { IsExist = true } };
            IList<IResult> resultDocumentDto = new List<IResult>() { new DocumentDto() { Body = new byte[] { 1, 1, 1 }, Name = "qwerty" } };

            _mockCommandDispatcher = new Mock<ICommandDispatcher>();
            _mockCommandDispatcher.Setup(disp => disp.Send(It.IsAny<AddDocumentCommand>()))
                                  .Returns(Task.FromResult(resultIdDto));
            _mockCommandDispatcher.Setup(disp => disp.Send(It.IsAny<DeleteDocumentCommand>()))
                                  .Returns(Task.FromResult(resultIdDto));
            _mockCommandDispatcher.Setup(disp => disp.Send(It.IsAny<UpdateDocumentCommand>()))
                                  .Returns(Task.FromResult(resultIdDto));

            _mockQueryDispatcher = new Mock<IQueryDispatcher>();
            _mockQueryDispatcher.Setup(disp => disp.Send(It.IsAny<GetHashOfDocumentQuery>()))
                                .Returns(Task.FromResult(resultHashDto));
            _mockQueryDispatcher.Setup(disp => disp.Send(It.IsAny<IsDocumentExitQuery>()))
                                .Returns(Task.FromResult(resultDocumentExistDto));
            _mockQueryDispatcher.Setup(disp => disp.Send(It.IsAny<GetDocumentByIdQuery>()))
                                .Returns(Task.FromResult(resultDocumentDto));
            _mockQueryDispatcher.Setup(disp => disp.Send(It.IsAny<GetDocumentByIdQuery>()))
                                .Returns(Task.FromResult(resultDocumentDto));
        }

        [Test]
        public async Task GetHash_IdVersion_OkObjectResult()
        {
            var controller = new DocumentController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);

            var result = await controller.GetHash(It.IsAny<Guid>(), It.IsAny<int>());

            Assert.That(result, Is.InstanceOf(typeof(OkObjectResult)));
        }

        [Test]
        public async Task Get_Id_RedirectToActionResult()
        {
            var controller = new DocumentController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);

            var result = await controller.Get(It.IsAny<Guid>());

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
        }

        [Test]
        public async Task GetLastVersion_Id_FileContentResult()
        {
            var controller = new DocumentController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);

            var result = await controller.GetLastVersion(It.IsAny<Guid>());

            Assert.That(result, Is.InstanceOf(typeof(FileContentResult)));
        }

        [Test]
        public async Task Get_IdVersion_FileContentResult()
        {
            var controller = new DocumentController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);

            var result = await controller.Get(It.IsAny<Guid>(), It.IsAny<int>());

            Assert.That(result, Is.InstanceOf(typeof(FileContentResult)));
        }

        [Test]
        public async Task Post_CreateDocumentRequest_CreatedResult()
        {
            var controller = new DocumentController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);

            var stream = new MemoryStream(new byte[] { 1, 1, 1 });
            var file = new FormFile(stream, 0, 3, "name", "fileName");

            var result = await controller.Post(new CreateDocumentRequest() { File = file });

            Assert.That(result, Is.InstanceOf(typeof(CreatedResult)));
        }

        [Test]
        public async Task Delete_Id_NoContentResult()
        {
            var controller = new DocumentController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);

            var result = await controller.Delete(It.IsAny<Guid>());

            Assert.That(result, Is.InstanceOf(typeof(NoContentResult)));
        }

        [Test]
        public async Task Put_UpdateDocumentRequest_NoContentResult()
        {
            var controller = new DocumentController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);

            var stream = new MemoryStream(new byte[] { 1, 1, 1 });
            var file = new FormFile(stream, 0, 3, "name", "fileName");

            var result = await controller.Put(new UpdateDocumentRequest() { Id = It.IsAny<Guid>(), CreaterId = It.IsAny<Guid>(), File = file });

            Assert.That(result, Is.InstanceOf(typeof(NoContentResult)));
        }
    }
}
