using Dapper;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Interfaces;

namespace SignatureService.DataAccess.DataBase.Realisations
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlServerConnectionProvider _provider;

        public UserRepository(SqlServerConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<Guid> AddUserAsync(UserEntity user)
        {
            var sql = $"INSERT INTO Users (Id, PublicKey, PrivateKey)" +
                      $"VALUES ({user.Id}, {user.PublicKey}, {user.PrivateKey})";

            using var connection = _provider.GetDbConnection();

            await connection.ExecuteAsync(sql);

            return user.Id;
        }
    }
}
