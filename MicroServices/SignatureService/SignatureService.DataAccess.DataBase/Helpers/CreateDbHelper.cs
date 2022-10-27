using Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace SignatureService.DataAccess.DataBase.Helpers
{
    public static class CreateDbHelper
    {
        private static readonly string CreateScript =
                "CREATE TABLE IF NOT EXISTS users ( " +
                    "Id UNIQUEIDENTIFIER NOT NULL, " +
                    "PublicKey VARBINARY(MAX) NOT NULL, " +
                    "PrivateKey VARBINARY(MAX) NOT NULL " +
                ");" +
                "CREATE TABLE IF NOT EXISTS signatures ( " +
                    "Id UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL, " +
                    "DocumentId UNIQUEIDENTIFIER NOT NULL, " +
                    "[Version] INT NOT NULL, " +
                    "[Hash] VARBINARY(MAX) NOT NULL, " +
                    "UserId UNIQUEIDENTIFIER NOT NULL" +
                ");";

        public static async Task CreateDb(ServiceProvider services)
        {
            var connetctionProvider = services.GetRequiredService<SqlServerConnectionProvider>();

            var connection = connetctionProvider.GetDbConnection();

            await connection.ExecuteAsync(CreateScript);
        }
    }
}
