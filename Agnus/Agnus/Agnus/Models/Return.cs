namespace Agnus.Models
{
    public class Return
    {
        public bool Result {  get; set; }
        public string Message { get; set; }
        public string? Error {  get; set; }
        public UserToken? UserToken { get; set; }
    }
}
