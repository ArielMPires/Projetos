namespace NexuSys.DTOs.Review_Defects
{
    public class ByReview_DefectsDTO
    {
        public int ID { get; set; }
        public int Review { get; set; }
        public int Defects { get; set; }
        public int History { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? DateChanged { get; set; }
    }
}
