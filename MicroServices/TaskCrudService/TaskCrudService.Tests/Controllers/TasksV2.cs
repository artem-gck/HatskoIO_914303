namespace TaskCrudService.Tests.Controllers
{
    [TestFixture]
    public class TasksV2
    {
        private Mock<IService<TaskEntity>> _mockTaskService;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<TaskService>> _mockLogger;
        private Mock<IValidator<CreateTaskRequest>> _mockCreateValidator;
        private Mock<IValidator<UpdateTaskRequest>> _mockUpdateValidator;
        private Mock<ISendEndpointProvider> _mockSendEndpointProvider;
        private Mock<IConfiguration> _mockConfig;

        [SetUp]
        public void Setup()
        {
            _mockTaskService = new Mock<IService<TaskEntity>>();
            _mockTaskService.Setup(taskService => taskService.UpdateAsync(It.IsAny<Guid>(), It.IsAny<TaskEntity>()))
                            .Returns(Task.CompletedTask);

            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(mapper => mapper.Map<TaskEntity>(It.IsAny<UpdateTaskRequest>()))
                       .Returns(It.IsAny<TaskEntity>());

            _mockLogger = new Mock<ILogger<TaskService>>();

            var validatonResult = new Mock<FluentValidation.Results.ValidationResult>();
            validatonResult.Setup(res => res.IsValid).Returns(true);

            _mockCreateValidator = new Mock<IValidator<CreateTaskRequest>>();
            _mockCreateValidator.Setup(val => val.ValidateAsync(It.IsAny<CreateTaskRequest>(), default))
                                .Returns(Task.FromResult(validatonResult.Object));

            _mockUpdateValidator = new Mock<IValidator<UpdateTaskRequest>>();
            _mockUpdateValidator.Setup(val => val.ValidateAsync(It.IsAny<UpdateTaskRequest>(), default))
                                .Returns(Task.FromResult(validatonResult.Object));

            var sendEndpoint = new Mock<ISendEndpoint>();
            sendEndpoint.Setup(s => s.Send(It.IsAny<UpdateTaskMessage>(), default))
                        .Returns(Task.CompletedTask);
            sendEndpoint.Setup(s => s.Send(It.IsAny<NewTaskMessage>(), default))
                        .Returns(Task.CompletedTask);

            _mockSendEndpointProvider = new Mock<ISendEndpointProvider>();
            _mockSendEndpointProvider.Setup(prov => prov.GetSendEndpoint(It.IsAny<Uri>()))
                                     .Returns(Task.FromResult(sendEndpoint.Object));

            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "ServiceBus")]).Returns("Endpoint=sb://document-flow.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=GxezeZbnDkhLbOUucalKWZ4D1+CaYbNigyxPgH1DSB4=");

            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["Queues:UpdateTask"])
                       .Returns(It.IsAny<string>());
            _mockConfig.Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings")))
                       .Returns(mockConfSection.Object);
        }

        [Test]
        public async Task Put_UpdateTaskRequest_NoContentResult()
        {
            var controller = new TaskControllerV2(_mockTaskService.Object, _mockMapper.Object, _mockLogger.Object, _mockCreateValidator.Object, _mockUpdateValidator.Object, _mockSendEndpointProvider.Object, _mockConfig.Object);

            var result = await controller.Put(It.IsAny<Guid>(), new UpdateTaskRequest()
            {
                OwnerUserId = It.IsAny<Guid>(),
                DeadLine = It.IsAny<DateTime>(),
                Header = It.IsAny<string>()
            });

            Assert.That(result, Is.InstanceOf(typeof(NoContentResult)));
        }

        [Test]
        public async Task Post_CreateTaskRequest_CreatedResult()
        {
            var controller = new TaskControllerV2(_mockTaskService.Object, _mockMapper.Object, _mockLogger.Object, _mockCreateValidator.Object, _mockUpdateValidator.Object, _mockSendEndpointProvider.Object, _mockConfig.Object);

            var result = await controller.Post(new CreateTaskRequest()
            {
                OwnerUserId = It.IsAny<Guid>(),
                DeadLine = It.IsAny<DateTime>(),
                Header = It.IsAny<string>()
            });

            Assert.That(result, Is.InstanceOf(typeof(CreatedResult)));
        }
    }
}
