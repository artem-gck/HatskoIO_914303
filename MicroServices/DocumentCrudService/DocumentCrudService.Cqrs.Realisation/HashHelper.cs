using System.Security.Cryptography;

namespace DocumentCrudService.Cqrs.Realisation
{
    public static class HashHelper
    {
        public static byte[] GetMD5Hash(this byte[] file)
        {
            using var md5 = MD5.Create();
            using var stream = new MemoryStream(file);

            return md5.ComputeHash(stream);
        }
    }
}
