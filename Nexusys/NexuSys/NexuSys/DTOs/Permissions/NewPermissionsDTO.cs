namespace NexuSys.DTOs.Permissions
{
    public class NewPermissionsDTO
    {
        public int ID { get; set; }
        public int Role { get; set; }
        public string Page { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
