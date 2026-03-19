namespace NexuSys.DTOs.Files
{
    public class NewFileDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? Folder { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
