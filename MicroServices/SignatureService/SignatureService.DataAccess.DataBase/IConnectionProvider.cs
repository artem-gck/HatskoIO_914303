using System.Data;

namespace SignatureService.DataAccess.DataBase
{
    public interface IConnectionProvider
    {
        public IDbConnection GetDbConnection();
    }
}
