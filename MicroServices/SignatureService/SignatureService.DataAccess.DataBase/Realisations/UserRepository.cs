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
            var sql = $"INSERT INTO users (Id, PublicKey, PrivateKey) " +
                      $"VALUES ('{user.Id}', @Public, @Private)";

            using var connection = _provider.GetDbConnection();

            await connection.ExecuteAsync(sql, new { Public = user.PublicKey, Private = user.PrivateKey});

            return user.Id;
        }

        public async Task<UserEntity> GetAsync(Guid id)
        {
            var sql = $"SELECT * " +
                      $"FROM users " +
                      $"WHERE Id = '{id}'";

            using var connection = _provider.GetDbConnection();

            var user = await connection.QueryFirstOrDefaultAsync<UserEntity>(sql);

            return user;
        }
    }
}
