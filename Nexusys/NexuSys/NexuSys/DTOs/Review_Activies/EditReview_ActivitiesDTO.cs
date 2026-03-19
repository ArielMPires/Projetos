namespace NexuSys.DTOs.Review_Activies
{
    public class EditReview_ActivitiesDTO
    {
        public int ID { get; set; }
        public string Review { get; set; }
        public int Activities { get; set; }
        public int History { get; set; }
        public string Repair_Activities { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
