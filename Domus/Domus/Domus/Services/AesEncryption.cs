using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Domus.Services
{
    public class AesEncryption
    {
        private static readonly byte[] Key;
        private static readonly byte[] IV;

        static AesEncryption()
        {
            // Lê uma vez, na inicialização do app
            Key = Encoding.UTF8.GetBytes(
                Environment.GetEnvironmentVariable("AES_KEY")
            );

            IV = Encoding.UTF8.GetBytes(
                Environment.GetEnvironmentVariable("AES_IV")
            );
        }

        public static string Encrypt(string plain)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
                sw.Write(plain);

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string encrypted)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(encrypted));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
