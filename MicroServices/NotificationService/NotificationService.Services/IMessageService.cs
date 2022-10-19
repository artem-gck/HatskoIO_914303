using NotificationService.Services.Dto;

namespace NotificationService.Services
{
    public interface IMessageService
    {
        public Task<IEnumerable<MessageDto>> GetAsync(int page, int count);
        public Task<MessageDto> GetAsync(Guid id);
    }
}
