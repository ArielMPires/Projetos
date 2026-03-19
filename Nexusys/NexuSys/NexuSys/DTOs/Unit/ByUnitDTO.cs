namespace NexuSys.DTOs.Unit
{
    public class ByUnitDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
        public int History { get; set; }
    }
}
