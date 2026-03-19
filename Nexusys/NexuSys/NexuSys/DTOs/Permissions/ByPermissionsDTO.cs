namespace NexuSys.DTOs.Permissions
{
    public class ByPermissionsDTO
    {
        public int ID { get; set; }
        public int Role { get; set; }
        public string Page { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
