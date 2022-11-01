using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace DocumentCrudService.Repositories.Realisation.Context
{
    public class DocumentContext
    {
        public IMongoDatabase Database { get; set; }
        public IGridFSBucket GridFS { get; set; }

        public DocumentContext(IConfiguration configuration)
        {
            string connectionString = Environment.GetEnvironmentVariable("DocumentsConnection") ?? configuration.GetConnectionString("DocumentsConnection");
            var connection = new MongoUrlBuilder(connectionString);
            var client = new MongoClient(connectionString);

            Database = client.GetDatabase(connection.DatabaseName);
            GridFS = new GridFSBucket(Database);
        }
    }
}
