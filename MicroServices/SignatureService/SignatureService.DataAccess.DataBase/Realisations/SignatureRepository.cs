using Dapper;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Exceptiions;
using SignatureService.DataAccess.DataBase.Interfaces;
using static Dapper.SqlMapper;

namespace SignatureService.DataAccess.DataBase.Realisations
{
    public class SignatureRepository : ISignatureRepository
    {
        private readonly SqlServerConnectionProvider _provider;

        public SignatureRepository(SqlServerConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task AddAsync(SignatureEntity entity)
        {
            var sql = $"INSERT INTO signatures (DocumentId, Version, Hash, UserId) " +
                      $"VALUES ('{entity.DocumentId}', {entity.Version}, @Hash, '{entity.UserId}')";

            using var connection = _provider.GetDbConnection();
            await connection.ExecuteAsync(sql, new { Hash = entity.Hash });
        }

        public async Task<IEnumerable<SignatureEntity>> GetByDocumentIdAsync(Guid id, int version)
        {
            var sql = $"SELECT * " +
                      $"FROM signatures " +
                      $"WHERE DocumentId = '{id}' AND Version = {version} " +
                      $"JOIN users ON signatures.UserId = users.Id";

            using var connection = _provider.GetDbConnection();
            var signatureEntity = await connection.QueryAsync<SignatureEntity, UserEntity, SignatureEntity>(sql, (signature, user) =>
            {
                signature.User = user;
                return signature;
            },
            splitOn: "UserId");

            if (signatureEntity is null || signatureEntity.Count() == 0)
                throw new NotFoundException(id, version);

            return signatureEntity;
        }

        public async Task<IEnumerable<byte[]>> GetDocumentHashes(Guid id, int version)
        {
            var sql = "SELECT Hash " +
                      "FROM signatures " +
                     $"WHERE DocumentId = '{id}' AND Version = {version}";

            using var connection = _provider.GetDbConnection();
            var hashes = await connection.QueryAsync<byte[]>(sql);

            return hashes;
        }
    }
}
