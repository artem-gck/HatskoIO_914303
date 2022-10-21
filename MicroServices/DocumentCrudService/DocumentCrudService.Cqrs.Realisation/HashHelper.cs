using System.Security.Cryptography;

namespace DocumentCrudService.Cqrs.Realisation
{
    public static class HashHelper
    {
        public static byte[] GetMD5Hash(this byte[] file)
        {
            using var alg = SHA256.Create();

            return alg.ComputeHash(file);
        }
    }
}
