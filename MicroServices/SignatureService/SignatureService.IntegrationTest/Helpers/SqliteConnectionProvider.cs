using Microsoft.Data.Sqlite;
using SignatureService.DataAccess.DataBase;
using System.Data;

namespace SignatureService.IntegrationTest.Helpers
{
    public class SqliteConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionString;

        public SqliteConnectionProvider(string connectionString)
            => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public virtual IDbConnection GetDbConnection()
            => new SqliteConnection(_connectionString);
    }
}
