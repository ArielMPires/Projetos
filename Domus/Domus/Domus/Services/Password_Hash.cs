using System.Security.Cryptography;
using System.Text;

namespace Domus.Services
{
    public class Password_Hash
    {
        private HashAlgorithm _algorithm;

        public Password_Hash(HashAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        public string PasswordEncrypt(string password)
        {
            var encodedValue= Encoding.UTF8.GetBytes(password);
            var encryptedPassword = _algorithm.ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var caracter in encryptedPassword)
            {
                sb.Append(caracter.ToString("X2"));
            }

            return sb.ToString();
        }

        public bool CompareHash(string PasswordTyped, string PasswordCurrent) => PasswordEncrypt(PasswordTyped) == PasswordCurrent;

    }
}
