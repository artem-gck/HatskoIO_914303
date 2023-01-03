using DocumentCrudService.Repositories.DbServices;
using DocumentCrudService.Repositories.Entities;
using DocumentCrudService.Repositories.Realisation.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;

namespace DocumentCrudService.Repositories.Realisation
{
    public class DocumentNameRepository : IDocumentNameRepository
    {
        private readonly DocumentContext _documentContext;

        public DocumentNameRepository(DocumentContext documentContext)
        {
            _documentContext = documentContext ?? throw new ArgumentNullException(nameof(documentContext));
        }

        public async Task<IEnumerable<DocumentNameEntity>> GetAsync(int numberOfPage, int countOnPage)
        {
            var filter = new BsonDocument();
            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);

            var options = new GridFSFindOptions { Sort = sort };

            var cursor = await _documentContext.GridFS.FindAsync(filter, options);
            var listOfFileInfo = new List<DocumentNameEntity>();

            foreach (var item in await cursor.ToListAsync())
            {

                item.Metadata.TryGetValue("Id", out BsonValue idValue);
                item.Metadata.TryGetValue("Version", out BsonValue version);
                item.Metadata.TryGetValue("CreaterId", out BsonValue creatorId);

                var info = new DocumentNameEntity()
                {
                    Id = new Guid(idValue.AsString),
                    Version = (int)version,
                    CreatorId = new Guid(creatorId.AsString),
                    FileName = item.Filename,
                    UploadDate = item.UploadDateTime
                };

                listOfFileInfo.Add(info);
            }

            return listOfFileInfo.GroupBy(f => f.Id).Select(g => new DocumentNameEntity()
            {
                Id = g.Key,
                Version = g.OrderByDescending(x => x.Version).First().Version,
                FileName = g.OrderByDescending(x => x.Version).First().FileName,
                CreatorId= g.OrderByDescending(x => x.Version).First().CreatorId,
                UploadDate = g.OrderByDescending(x => x.Version).First().UploadDate,
            }).Skip((numberOfPage - 1) * countOnPage).Take(countOnPage);
        }

        public async Task<IEnumerable<DocumentNameEntity>> GetByUserIdAsync(Guid userId, int? count)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("metadata.CreaterId", userId.ToString());
            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);

            var options = new GridFSFindOptions { Sort = sort, Limit = count };

            var cursor = await _documentContext.GridFS.FindAsync(filter, options);
            var listOfFileInfo = new List<DocumentNameEntity>();

            foreach (var item in await cursor.ToListAsync())
            {
                item.Metadata.TryGetValue("Id", out BsonValue idValue);
                item.Metadata.TryGetValue("Version", out BsonValue version);
                item.Metadata.TryGetValue("CreaterId", out BsonValue creatorId);

                var info = new DocumentNameEntity()
                {
                    Id = new Guid(idValue.AsString),
                    Version = (int)version,
                    CreatorId = new Guid(creatorId.AsString),
                    FileName = item.Filename,
                    UploadDate = item.UploadDateTime
                };

                listOfFileInfo.Add(info);
            }

            return listOfFileInfo.GroupBy(f => f.Id).Select(g => new DocumentNameEntity()
            {
                Id = g.Key,
                Version = g.OrderByDescending(x => x.Version).First().Version,
                FileName = g.OrderByDescending(x => x.Version).First().FileName,
                CreatorId = g.OrderByDescending(x => x.Version).First().CreatorId,
                UploadDate = g.OrderByDescending(x => x.Version).First().UploadDate,
            });
        }

        public async Task<DocumentNameEntity> GetByDocumentIdAsync(Guid id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("metadata.Id", id.ToString());
            var sort = Builders<GridFSFileInfo>.Sort.Descending(x => x.UploadDateTime);

            var options = new GridFSFindOptions { Sort = sort };

            var cursor = await _documentContext.GridFS.FindAsync(filter, options);
            var fileInfo = new DocumentNameEntity();

            var firstOfCursor = cursor.FirstOrDefault();

            firstOfCursor.Metadata.TryGetValue("Id", out BsonValue idValue);
            firstOfCursor.Metadata.TryGetValue("Version", out BsonValue version1);
            firstOfCursor.Metadata.TryGetValue("CreaterId", out BsonValue creatorId);

            var info = new DocumentNameEntity()
            {
                Id = new Guid(idValue.AsString),
                Version = (int)version1,
                CreatorId = new Guid(creatorId.AsString),
                FileName = firstOfCursor.Filename,
                UploadDate = firstOfCursor.UploadDateTime
            };

            fileInfo = info;

            return fileInfo;
        }
    }
}
