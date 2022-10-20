using Microsoft.Data.SqlClient;
using System.Data;

namespace SignatureService.DataAccess.DataBase
{
    public class SqlServerConnectionProvider
    {
        private readonly string _connectionString;

        public SqlServerConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
