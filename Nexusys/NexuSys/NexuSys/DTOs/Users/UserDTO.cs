namespace NexuSys.DTOs.Users
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public bool MostrarSenha { get; internal set; } = false;
    }
}
