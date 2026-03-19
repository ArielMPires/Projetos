namespace Domus.DTO.Users
{
    public class CreateUserDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? Role { get; set; }
        public int? Department { get; set; }
        public byte[]? photo { get; set; }
        public byte[]? Signature { get; set; }
    }
}
