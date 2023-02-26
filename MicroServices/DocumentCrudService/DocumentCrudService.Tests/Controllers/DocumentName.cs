using DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocuments;
using DocumentCrudService.Cqrs.Realisation.Queries.GetAllNamesOfDocumentsByUserId;
using DocumentCrudService.Cqrs.Realisation.Queries.GetByDocumentId;

namespace DocumentCrudService.Tests.Controllers
{
    public class DocumentName
    {
        private Mock<IQueryDispatcher> _mockQueryDispatcher;

        [SetUp]
        public void Setup()
        {
            IList<IResult> resultDocumentNameDto = new List<IResult>() { new DocumentNameDto() { Name = It.IsAny<string>(), Version = It.IsAny<int>(), CreatorId = It.IsAny<Guid>(), Id = It.IsAny<Guid>(), UploadDate = It.IsAny<DateTime>() } };

            _mockQueryDispatcher = new Mock<IQueryDispatcher>();
            _mockQueryDispatcher.Setup(disp => disp.Send(It.IsAny<GetAllNamesOfDocumentsQuery>()))
                                .Returns(Task.FromResult(resultDocumentNameDto));
            _mockQueryDispatcher.Setup(disp => disp.Send(It.IsAny<GetAllNamesOfDocumentsByUserIdQuery>()))
                                .Returns(Task.FromResult(resultDocumentNameDto));
            _mockQueryDispatcher.Setup(disp => disp.Send(It.IsAny<GetByDocumentIdQuery>()))
                                .Returns(Task.FromResult(resultDocumentNameDto));
        }

        [Test]
        public async Task Get_PageCount_OkObjectResult()
        {
            var controller = new DocumentNameController(_mockQueryDispatcher.Object);

            var result = await controller.Get(It.IsAny<int>(), It.IsAny<int>());

            Assert.That(result, Is.InstanceOf(typeof(OkObjectResult)));
        }

        [Test]
        public async Task Get_UserIdCount_OkObjectResult()
        {
            var controller = new DocumentNameController(_mockQueryDispatcher.Object);

            var result = await controller.Get(It.IsAny<Guid>(), It.IsAny<int>());

            Assert.That(result, Is.InstanceOf(typeof(OkObjectResult)));
        }

        [Test]
        public async Task GetById_Id_OkObjectResult()
        {
            var controller = new DocumentNameController(_mockQueryDispatcher.Object);

            var result = await controller.GetById(It.IsAny<Guid>());

            Assert.That(result, Is.InstanceOf(typeof(OkObjectResult)));
        }
    }
}
