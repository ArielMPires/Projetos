namespace Agnus.DTO.Passwords
{
    public class PasswordByDTO
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public int Owner { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int? CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
