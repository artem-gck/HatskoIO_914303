using DocumentCrudService.Application.DbServices;
using DocumentCrudService.Domain.Entities;
using DocumentCrudService.Domain.Exceptions;
using DocumentCrudService.Infrastructure.DbRrealisation.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace DocumentCrudService.Infrastructure.DbRrealisation
{
    public class DoucmentNameRepository : IDoucmentNameRepository
    {
        private readonly DocumentContext _documentContext;

        public DoucmentNameRepository(DocumentContext documentContext)
        {
            _documentContext = documentContext ?? throw new ArgumentNullException(nameof(documentContext));
        }

        public async Task<IEnumerable<DocumentNameEntity>> GetAllAsync()
        {
            var filter = Builders<GridFSFileInfo>.Filter.And();

            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);

            var options = new GridFSFindOptions
            {
                Sort = sort
            };

            var cursor = await _documentContext.GridFS.FindAsync(filter, options);
            var listOfFileInfo = (await cursor.ToListAsync()).Select(info => new DocumentNameEntity()
            {
                Id = info.Id.ToString(),
                FileName = info.Filename,
                UploadDate = info.UploadDateTime
            });

            return listOfFileInfo;
        }

        public async Task<DocumentNameEntity> GetAsync(string id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.And(Builders<GridFSFileInfo>.Filter.Eq(x => x.Id, new ObjectId(id)));

            var options = new GridFSFindOptions
            {
                Limit = 1
            };

            var cursor = await _documentContext.GridFS.FindAsync(filter, options);
            var fileInfo = (await cursor.ToListAsync()).FirstOrDefault();

            if (fileInfo is null)
                throw new DocumentNotFoundException(id);

            var documentNameEntity = new DocumentNameEntity()
            {
                Id = fileInfo.Id.ToString(),
                FileName = fileInfo.Filename,
                UploadDate = fileInfo.UploadDateTime
            };

            return documentNameEntity;
        }

        public async Task<byte[]> GetByNameAsync(string fileName, int version = -1)
        {
            var options = new GridFSDownloadByNameOptions
            {
                Revision = version
            };

            var document = await _documentContext.GridFS.DownloadAsBytesByNameAsync(fileName, options);

            return document;
        }
    }
}
