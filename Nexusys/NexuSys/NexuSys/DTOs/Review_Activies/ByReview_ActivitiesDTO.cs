namespace NexuSys.DTOs.Review_Activies
{
    public class ByReview_ActivitiesDTO
    {
        public int ID { get; set; }
        public int Review { get; set; }
        public int Activities { get; set; }
        public int History { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? DateChanged { get; set; }
    }
}
