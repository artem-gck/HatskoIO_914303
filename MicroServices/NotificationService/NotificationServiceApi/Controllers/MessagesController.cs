using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Services;
using NotificationServiceApi.ViewModels;

namespace NotificationServiceApi.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessagesController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets the specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="count">The count.</param>
        /// <returns>Enumerable of messages</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/messages?page=1 count=10
        ///
        /// </remarks>
        /// <response code="200">Return models</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<MessageResponce>> Get(int page = 1, int count = 10)
            => _mapper.Map<IEnumerable<MessageResponce>>(await _messageService.GetAsync(page, count));

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of message.</param>
        /// <returns>Message</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/messages/{id}
        ///
        /// </remarks>
        /// <response code="200">Return model</response>
        /// <response code="404">Message not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<MessageResponce> Get(Guid id)
            => _mapper.Map<MessageResponce>(await _messageService.GetAsync(id));
    }
}
