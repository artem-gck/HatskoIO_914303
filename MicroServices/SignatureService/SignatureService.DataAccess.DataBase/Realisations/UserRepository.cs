using Dapper;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Exceptiions;
using SignatureService.DataAccess.DataBase.Interfaces;

namespace SignatureService.DataAccess.DataBase.Realisations
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionProvider _provider;

        public UserRepository(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task<Guid> AddAsync(UserEntity user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));

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

            if (user is null)
                throw new NotFoundException(id);

            return user;
        }
    }
}
