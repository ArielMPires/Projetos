using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domus.Services
{
    public class EncryptedStringConverter : ValueConverter<string, string>
    {
        public EncryptedStringConverter()
            : base(
                v => v == null ? null : AesEncryption.Encrypt(v),
                v => v == null ? null : AesEncryption.Decrypt(v)
            )
        { }
    }
}
