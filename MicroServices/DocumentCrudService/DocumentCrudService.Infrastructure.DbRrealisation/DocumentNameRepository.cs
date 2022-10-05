using DocumentCrudService.Repositories.DbServices;
using DocumentCrudService.Repositories.Entities;
using DocumentCrudService.Repositories.Realisation.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace DocumentCrudService.Repositories.Realisation
{
    public class DocumentNameRepository : IDocumentNameRepository
    {
        private readonly DocumentContext _documentContext;

        public DocumentNameRepository(DocumentContext documentContext)
        {
            _documentContext = documentContext ?? throw new ArgumentNullException(nameof(documentContext));
        }

        public async Task<IEnumerable<DocumentNameEntity>> GetAsync()
        {
            var filter = new BsonDocument();
            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);
            var options = new GridFSFindOptions { Sort = sort };

            var cursor = await _documentContext.GridFS.FindAsync(filter, options);
            var listOfFileInfo = (await cursor.ToListAsync()).Select(info => new DocumentNameEntity()
            {
                Id = info.Id.ToString(),
                FileName = info.Filename,
                UploadDate = info.UploadDateTime
            });

            return listOfFileInfo;
        }
    }
}
