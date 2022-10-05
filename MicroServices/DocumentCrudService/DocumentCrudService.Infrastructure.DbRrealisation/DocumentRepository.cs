using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using DocumentCrudService.Repositories.Realisation.Context;
using DocumentCrudService.Repositories.Entities;
using DocumentCrudService.Repositories.Exceptions;
using DocumentCrudService.Repositories.DbServices;

namespace DocumentCrudService.Repositories.Realisation
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentContext _documentContext;

        public DocumentRepository(DocumentContext documentContext)
        {
            _documentContext = documentContext ?? throw new ArgumentNullException(nameof(documentContext));
        }

        public async Task AddAsync(byte[] document, string fileName)
        {
            await _documentContext.GridFS.UploadFromBytesAsync(fileName, document);
        }

        public async Task DeleteAsync(string id)
        {
            await _documentContext.GridFS.DeleteAsync(new ObjectId(id));
        }

        public async Task<DocumentEntity> GetAsync(string id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", new ObjectId(id));

            try
            {
                var document = await _documentContext.GridFS.DownloadAsBytesAsync(new ObjectId(id));

                var cursor = await _documentContext.GridFS.FindAsync(filter);
                var fileInfo = (await cursor.ToListAsync()).FirstOrDefault();

                var documentEntity = new DocumentEntity()
                {
                    FileName = fileInfo.Filename,
                    File = document
                };

                return documentEntity;
            }
            catch (IndexOutOfRangeException)
            {
                throw new DocumentNotFoundException(id);
            }
        }

        public async Task<DocumentEntity> GetByNameAsync(string fileName, int version = -1)
        {
            try
            {
                var options = new GridFSDownloadByNameOptions { Revision = version };

                var document = await _documentContext.GridFS.DownloadAsBytesByNameAsync(fileName, options);

                var documentEntity = new DocumentEntity()
                {
                    FileName = fileName,
                    File = document
                };

                return documentEntity;
            }
            catch (IndexOutOfRangeException)
            {
                throw new DocumentNotFoundException(fileName);
            }
        }

        public async Task UpdateAsync(byte[] document, string fileName)
        {
            await _documentContext.GridFS.UploadFromBytesAsync(fileName, document);
        }
    }
}
