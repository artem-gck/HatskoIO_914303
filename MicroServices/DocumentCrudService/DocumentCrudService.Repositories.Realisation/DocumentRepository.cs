using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using DocumentCrudService.Repositories.Realisation.Context;
using DocumentCrudService.Repositories.Entities;
using DocumentCrudService.Repositories.Exceptions;
using DocumentCrudService.Repositories.DbServices;
using Microsoft.Extensions.Logging;

namespace DocumentCrudService.Repositories.Realisation
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocumentContext _documentContext;
        private readonly ILogger<DocumentRepository> _logger;

        public DocumentRepository(DocumentContext documentContext, ILogger<DocumentRepository> logger)
        {
            _documentContext = documentContext ?? throw new ArgumentNullException(nameof(documentContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));    
        }

        public async Task<string> AddAsync(byte[] document, string fileName)
        {
            var id = await _documentContext.GridFS.UploadFromBytesAsync(fileName, document);

            _logger.LogDebug("Add document with id = {Id}", id);

            return id.ToString();
        }

        public async Task DeleteAsync(string id)
        {
            await _documentContext.GridFS.DeleteAsync(new ObjectId(id));

            _logger.LogDebug("Delete document with id = {Id}", id);
        }

        public async Task<DocumentEntity> GetAsync(string id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", new ObjectId(id));

            try
            {
                var document = await _documentContext.GridFS.DownloadAsBytesAsync(new ObjectId(id));

                _logger.LogDebug("Get document with id = {Id}", id);

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
                var exception = new DocumentNotFoundException(id);

                _logger.LogWarning(exception, "No document with id = {Id}", id);

                throw exception;
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

                _logger.LogDebug("Gett document with name = {FileName}", documentEntity.FileName);

                return documentEntity;
            }
            catch (IndexOutOfRangeException)
            {
                var exception = new DocumentNotFoundException(fileName);

                _logger.LogWarning(exception, "No document with name = {FileName}", fileName);

                throw exception;
            }
        }

        public async Task UpdateAsync(byte[] document, string fileName)
        {
            var id = await _documentContext.GridFS.UploadFromBytesAsync(fileName, document);

            _logger.LogDebug("Update document, new id = {id}", id);
        }
    }
}
