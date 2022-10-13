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

        public async Task<bool> IsDocumentExit(Guid id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("metadata.Id", id.ToString());
            var cursor = await _documentContext.GridFS.FindAsync(filter);

            var listOfFileInfo = await cursor.ToListAsync();

            return listOfFileInfo.Any();
        }

        public async Task<Guid> AddAsync(Guid createrId, byte[] document, string fileName)
        {
            var id = Guid.NewGuid();

            var doc = new BsonDocument
            {
                {"Id", id.ToString()},
                {"CreaterId", createrId.ToString()},
                {"EditedAtDate", DateTime.Now.ToString()},
                {"Version", 0}
            };

            var meta = new GridFSUploadOptions() { Metadata = doc };

            await _documentContext.GridFS.UploadFromBytesAsync(fileName, document, meta);

            _logger.LogDebug("Add document with id = {Id}", id);

            return id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("metadata.Id", id.ToString());
            var cursor = await _documentContext.GridFS.FindAsync(filter);

            var listOfFileInfo = await cursor.ToListAsync();

            if (listOfFileInfo is null)
            {
                var exception = new DocumentNotFoundException(id.ToString());

                _logger.LogWarning(exception, "No document with id = {Id}", id);

                throw exception;
            }

            foreach(var doc in listOfFileInfo)
                await _documentContext.GridFS.DeleteAsync(doc.Id);

            _logger.LogDebug("Delete document with id = {Id}", id);
        }

        public async Task<DocumentEntity> GetAsync(Guid id, int version)
        {
            FilterDefinition<GridFSFileInfo> filter;

            if (version == -1)
                filter = Builders<GridFSFileInfo>.Filter.Eq("metadata.Id", id.ToString());
            else
                filter = Builders<GridFSFileInfo>.Filter.And(Builders<GridFSFileInfo>.Filter.Eq("metadata.Id", id.ToString()),
                                                             Builders<GridFSFileInfo>.Filter.Eq("metadata.Version", version));

            var options = new GridFSFindOptions()
            {
                Sort = Builders<GridFSFileInfo>.Sort.Descending("metadata.Version")
            };

            var cursor = await _documentContext.GridFS.FindAsync(filter, options);

            var fileInfo = (await cursor.ToListAsync()).FirstOrDefault();

            if (fileInfo is null)
            {
                var exception = new DocumentNotFoundException(id.ToString());

                _logger.LogWarning(exception, "No document with id = {Id}", id);

                throw exception;
            }

            var document = await _documentContext.GridFS.DownloadAsBytesAsync(fileInfo.Id);

            _logger.LogDebug("Get document with id = {Id}", id);

            var documentEntity = new DocumentEntity()
            {
                FileName = fileInfo.Filename,
                File = document
            };

            return documentEntity;
        }

        public async Task UpdateAsync(Guid id, Guid createrId, byte[] document, string fileName)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("metadata.Id", id.ToString());

            var options = new GridFSFindOptions()
            {
                Sort = Builders<GridFSFileInfo>.Sort.Descending("metadata.Version")
            };

            var cursor = await _documentContext.GridFS.FindAsync(filter, options);
            
            var fileInfo = (await cursor.ToListAsync()).FirstOrDefault();

            if (fileInfo is null)
            {
                var exception = new DocumentNotFoundException(id.ToString());

                _logger.LogWarning(exception, "No documents with id = {Id}", id);

                throw exception;
            }

            BsonElement version;

            fileInfo.Metadata.TryGetElement("Version", out version);

            var vers = version.Value.AsInt32 + 1;

            var doc = new BsonDocument
            {
                {"Id", id.ToString()},
                {"CreaterId", createrId.ToString()},
                {"EditedAtDate", DateTime.Now},
                {"Version", vers}
            };

            var meta = new GridFSUploadOptions() { Metadata = doc };

            await _documentContext.GridFS.UploadFromBytesAsync(fileName, document, meta);
        }
    }
}
