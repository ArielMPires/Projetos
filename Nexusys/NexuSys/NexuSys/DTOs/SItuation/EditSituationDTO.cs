namespace NexuSys.DTOs.SItuation
{
    public class EditSituationDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int History { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
