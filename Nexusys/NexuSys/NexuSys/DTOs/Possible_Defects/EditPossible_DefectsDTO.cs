namespace NexuSys.DTOs.Possible_Defects
{
    public class EditPossible_DefectsDTO
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
