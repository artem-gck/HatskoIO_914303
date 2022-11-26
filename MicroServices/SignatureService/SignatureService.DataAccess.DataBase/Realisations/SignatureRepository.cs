using Dapper;
using SignatureService.DataAccess.DataBase.Entities;
using SignatureService.DataAccess.DataBase.Exceptiions;
using SignatureService.DataAccess.DataBase.Interfaces;
using System.Reflection.Metadata;
using static Dapper.SqlMapper;

namespace SignatureService.DataAccess.DataBase.Realisations
{
    public class SignatureRepository : ISignatureRepository
    {
        private readonly IConnectionProvider _provider;

        public SignatureRepository(IConnectionProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public async Task AddAsync(SignatureEntity entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var sql = $"INSERT INTO signatures (DocumentId, Version, Hash, UserId) " +
                      $"VALUES ('{entity.DocumentId}', {entity.Version}, @Hash, '{entity.UserId}')";

            using var connection = _provider.GetDbConnection();

            await connection.ExecuteAsync(sql, new { Hash = entity.Hash, DocumentId = entity.DocumentId, Version = entity.Version, UserId = entity.UserId });
        }

        public async Task<IEnumerable<SignatureEntity>> GetByDocumentIdAsync(Guid id, int version)
        {
            if (version < -1)
                throw new ArgumentOutOfRangeException(nameof(version));

            var sql = $"SELECT * " +
                      $"FROM signatures " +
                      $"INNER JOIN users ON signatures.UserId = users.Id " +
                      $"WHERE DocumentId = '{id}' AND Version = {version}";

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
            if (version < -1)
                throw new ArgumentOutOfRangeException(nameof(version));

            var sql = "SELECT * " +
                      "FROM signatures " +
                     $"WHERE DocumentId = '{id}' AND Version = {version}";

            using var connection = _provider.GetDbConnection();
            var hashes = await connection.QueryAsync<SignatureEntity>(sql, null, null, null, null);

            if (hashes is null || hashes.Count() == 0)
                throw new NotFoundException(id, version);

            return hashes.Select(h => h.Hash);
        }
    }
}
