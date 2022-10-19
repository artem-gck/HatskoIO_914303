using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Services;
using NotificationServiceApi.ViewModels;
using System.Runtime.CompilerServices;

namespace NotificationServiceApi.Controllers
{
    [Route("messages")]
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

        [HttpGet]
        public async Task<IEnumerable<MessageResponce>> Get(int page = 1, int count = 10)
            => _mapper.Map<IEnumerable<MessageResponce>>(await _messageService.GetAsync(page, count));

        [HttpGet("{id}")]
        public async Task<MessageResponce> Get(Guid id)
            => _mapper.Map<MessageResponce>(await _messageService.GetAsync(id));
    }
}
