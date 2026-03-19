namespace Domus.DTO.Passwords
{
    public class NewPasswordDTO
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public int Owner { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int? CreateBy { get; set; }
        public DateTime DateCreateBy { get; set; }
    }
}
