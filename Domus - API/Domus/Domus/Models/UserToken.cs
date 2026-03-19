using Domus.Models.DB;

namespace Domus.Models
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
