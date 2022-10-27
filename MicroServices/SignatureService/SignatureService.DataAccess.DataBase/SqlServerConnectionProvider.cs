using Microsoft.Data.SqlClient;
using System.Data;

namespace SignatureService.DataAccess.DataBase
{
    public class SqlServerConnectionProvider
    {
        private readonly string _connectionString;

        public SqlServerConnectionProvider(string connectionString)
            => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public IDbConnection GetDbConnection()
            => new SqlConnection(_connectionString);
    }
}
