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
            var listOfFileInfo = new List<DocumentNameEntity>();

            foreach (var item in await cursor.ToListAsync())
            {
                BsonValue idValue;

                item.Metadata.TryGetValue("Id", out idValue);

                var info = new DocumentNameEntity()
                {
                    Id = new Guid(idValue.AsString),
                    FileName = item.Filename,
                    UploadDate = item.UploadDateTime
                };

                listOfFileInfo.Add(info);
            }

            return listOfFileInfo;
        }
    }
}
