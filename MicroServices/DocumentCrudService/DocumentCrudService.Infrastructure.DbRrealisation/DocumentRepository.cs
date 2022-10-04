using DocumentCrudService.Application.DbServices;
using DocumentCrudService.Domain.Exceptions;
using DocumentCrudService.Infrastructure.DbRrealisation.Context;
using MongoDB.Bson;

namespace DocumentCrudService.Infrastructure.DbRrealisation
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentContext _documentContext;

        public DocumentRepository(DocumentContext documentContext)
        {
            _documentContext = documentContext ?? throw new ArgumentNullException(nameof(documentContext));
        }

        public async Task<string> AddAsync(byte[] document, string fileName)
        {
            var documentId = await _documentContext.GridFS.UploadFromBytesAsync(fileName, document);

            return documentId.ToString();
        }

        public async Task<string> DeleteAsync(string id)
        {
            await _documentContext.GridFS.DeleteAsync(new ObjectId(id));

            return id;
        }

        public async Task<byte[]> GetAsync(string id)
        {
            try
            {
                var document = await _documentContext.GridFS.DownloadAsBytesAsync(new ObjectId(id));

                return document;
            }
            catch (IndexOutOfRangeException)
            {
                throw new DocumentNotFoundException(id);
            }
        }

        public async Task<string> UpdateAsync(byte[] document, string fileName)
        {
            var documentId = await _documentContext.GridFS.UploadFromBytesAsync(fileName, document);

            return documentId.ToString();
        }
    }
}
