namespace DocumentCrudService.Application.DbServices
{
    public interface IDocumentRepository
    {
        public Task<byte[]> GetAsync(string id);
        public Task<string> DeleteAsync(string id);
        public Task<string> AddAsync(byte[] document, string fileName);
        public Task<string> UpdateAsync(byte[] document, string fileName);
    }
}
