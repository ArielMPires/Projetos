namespace NexuSys.DTOs.Users
{
    public class NewUserDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public int Department { get; set; }
        public byte[] Photo { get; set; }
        public int Role { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
