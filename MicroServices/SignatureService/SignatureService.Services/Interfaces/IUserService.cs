namespace SignatureService.Services.Interfaces
{
    public interface IUserService
    {
        public Task<Guid> AddUserAsync(Guid id);
    }
}
