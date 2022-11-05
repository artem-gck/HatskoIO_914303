using AutoMapper;
using NotificationService.DataAccess.DataBase.Interfaces;
using NotificationService.Services.Dto;

namespace NotificationService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<MessageDto>> GetAsync(int page, int count)
            => _mapper.Map<IEnumerable<MessageDto>>(await _messageRepository.GetAsync(page, count));

        public async Task<MessageDto> GetAsync(Guid id)
            => _mapper.Map<MessageDto>(await _messageRepository.GetAsync(id));
    }
}
