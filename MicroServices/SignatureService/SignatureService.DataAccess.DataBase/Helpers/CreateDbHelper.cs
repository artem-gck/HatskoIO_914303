using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SignatureService.DataAccess.DataBase.Helpers
{
    public static class CreateDbHelper
    {
        private static readonly string CreateScriptSqlServer =
            "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='users' and xtype='U') " +
                "CREATE TABLE users ( " +
                    "Id UNIQUEIDENTIFIER NOT NULL, " +
                    "PublicKey VARBINARY(MAX) NOT NULL, " +
                    "PrivateKey VARBINARY(MAX) NOT NULL " +
                ") " +
            "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='signatures' and xtype='U') " +
                "CREATE TABLE signatures ( " +
                    "Id UNIQUEIDENTIFIER DEFAULT NEWID() NOT NULL, " +
                    "DocumentId UNIQUEIDENTIFIER NOT NULL, " +
                    "[Version] INT NOT NULL, " +
                    "[Hash] VARBINARY(MAX) NOT NULL, " +
                    "UserId UNIQUEIDENTIFIER NOT NULL" +
                ")";

        private static readonly string CreateScriptSqlite =
                "CREATE TABLE IF NOT EXISTS users ( " +
                    "Id NVARCHAR(1000) PRIMARY KEY, " +
                    "PublicKey BLOB NOT NULL, " +
                    "PrivateKey BLOB NOT NULL " +
                "); " +
                "CREATE TABLE IF NOT EXISTS signatures ( " +
                    "Id NVARCHAR(64) default (lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))),2) || '-' || substr('89ab',abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))),2) || '-' || lower(hex(randomblob(6)))) PRIMARY KEY, " +
                    "DocumentId NVARCHAR(1000) NOT NULL, " +
                    "Version INT NOT NULL, " +
                    "Hash BLOB NOT NULL, " +
                    "UserId NVARCHAR(1000) NOT NULL, " +
                    "FOREIGN KEY(UserId) REFERENCES users(Id) ON DELETE CASCADE" +
                "); ";

        public static async Task CreateDb(ServiceProvider services)
        {
            var connetctionProvider = services.GetRequiredService<IConnectionProvider>();

            var connection = connetctionProvider.GetDbConnection();

            if (connection is SqlServerConnectionProvider)
            {
                await connection.ExecuteAsync(CreateScriptSqlServer);
                return;
            }

            var byte1 = JsonSerializer.Deserialize<byte[]>("\"MIIBCgKCAQEAqhRT+l8zaqH2Jw5M/mhUt007XvAeAILCE9fx9xOvZxTAuHoWrepqdF6bNNJUWKcAya4f672TKaonxzusF19VLpWZgz2ELC074bMXb9FaIANz3TQ/gqoXJ246Zdbcz/TRhcrEvgjl+Lrx65rBRwZGGT6SslPpr8jDRjKkRIhs0OkDbmSALo1K9iZhJTn0kn8SkIk6tKTval917zTRAZyi98+mp02/lV57d0P7AIraFSOcbHZwLe0TQjMYRrojQx3LI9p6RUOxWkN35u+Zc9graA9iLinISFZgWgHfc6OZfXa6mxcbcU+IMsdIB9VhZ6zu+wiy6H2aIRg0gne+zOhJ1QIDAQAB\"");
            var byte2 = JsonSerializer.Deserialize<byte[]>("\"MIIEogIBAAKCAQEAqhRT+l8zaqH2Jw5M/mhUt007XvAeAILCE9fx9xOvZxTAuHoWrepqdF6bNNJUWKcAya4f672TKaonxzusF19VLpWZgz2ELC074bMXb9FaIANz3TQ/gqoXJ246Zdbcz/TRhcrEvgjl+Lrx65rBRwZGGT6SslPpr8jDRjKkRIhs0OkDbmSALo1K9iZhJTn0kn8SkIk6tKTval917zTRAZyi98+mp02/lV57d0P7AIraFSOcbHZwLe0TQjMYRrojQx3LI9p6RUOxWkN35u+Zc9graA9iLinISFZgWgHfc6OZfXa6mxcbcU+IMsdIB9VhZ6zu+wiy6H2aIRg0gne+zOhJ1QIDAQABAoIBAHXHbgBMPQby8ctKE/d5uHDXgu3TynMAGfYz7NP1RdpUfMFDisEVPHMpsMF7hf1+aQVBF8ngljCcLL/DiwEEe7Cu2IgR6Z3OFVHO+8PrbkYNHgdpTzHlJ/OeWcJ8cJ1yJEZKjMQs1VTR4QYPPRk6NwD0XBIyGfopRVEvcKIk93YTRZVW4ilSm2o+ve3ptAgIuFeQ4douDY8q5yDvYRSw3mThbm2BHxn9zwH0CdCzMHkskJ3OgmkBY7LKfcViej05e8bBkopkHe/YI9bLYbulvJJXqsc72XpJdIisFJ9H8nZRn4M1CHzePDvBTfFy7YJx5oZho986op/+zEK4Sfoa1mECgYEAyDFZkAhPBKeps2EHqUj3iS9UijVc42j3DwXdwufe+ek3GWevzpRq0poy6MTtO8N90ZRlcphLcHpSira/HZRY8pqlGtRCPL8OyErjblyadakK+kgzXKpGlVTZJYEWTimfKYNfjoo7sBrv5elSGleA0Xl8S3/ksVIIPMRSMrp/bA8CgYEA2X30ModT3QALujHmYZHW9NK8a6mnwryK+mgnEyG3mJlE6h+3JBGe6ERXzr4jGDmRKN+P7csKB+4E1MWiTnkAuPJG39kHXXq51QZp+30A1WyX2U5kV+hpsNXF9fI11IU7ZdWsBb1MSmLazaOVplbMQxgtoGXaYHfe79ZO/dI2l9sCgYAkykz3V0+OP3HEcXSC+9Hh+DzFGYF0mXmt3RogE5S6wM1Lce6xPD1VbtkkReyDKy4YMfli0fIrPY+lxmfg+75M93aYM6HooAQLcxfQ1Fvee650yIgH45MNJoaxicObobKKYoZRH75QYuLrkbFw/dvCo1zgUySoIbc506p3gCaiqQKBgGsbt25G7QsSYbQhrPZyDy1ktvxCgebKWZs2PcATQ8p3+NNgKR4vO1Xhimi7hKcCerVpXAVcj6UWF/T9G5CP0MZEMpk81X642NnLHdiHWCjDIQkYrRwJzjsTHDmiPdZagsUE9IGjFvYvNtg87o8Lge6s8pNidag6gGUW4rHnm5EvAoGAEXje+EeFLP8D1zB3ymQC7O3NoqM+mdOIvn55Yb4l82bkAgWWODcl6WBellceZalKhnkUtPSjPfPuAWUsgk3aVUMnGNaVLnAFNuV0Uoz3p9zQ92A0UcdPI/KhX3ZlJa/wBHwo6fAPMtXTbtf58d3qC4DLDNFiwv9LKJXGWmw1vzs=\"");

            var insertScript =
                $"INSERT INTO users (Id, PublicKey, PrivateKey) " +
                $"VALUES ('0be409a4-d101-4242-84bc-1a6695d37e89', @Str1, @Str2); " +
                $"INSERT INTO users (Id, PublicKey, PrivateKey) " +
                $"VALUES ('bb7db367-dbf8-400c-a01d-258c344ab501', @Str1, @Str2); ";

            var deleteScript = "DELETE FROM users; DELETE FROM signatures;";

            await connection.ExecuteAsync(CreateScriptSqlite);
            await connection.ExecuteAsync(deleteScript);
            await connection.ExecuteAsync(insertScript, new { Str1 = byte1, Str2 = byte2 });
        }
    }
}
