namespace Agnus.DTO.Passwords
{
    public class PasswordDTO
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Owner { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
