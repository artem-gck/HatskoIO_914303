using Microsoft.Data.SqlClient;
using System.Data;

namespace SignatureService.DataAccess.DataBase
{
    public class SqlServerConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionString;

        public SqlServerConnectionProvider(string connectionString)
            => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public virtual IDbConnection GetDbConnection()
            => new SqlConnection(_connectionString);
    }
}
