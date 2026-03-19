namespace NexuSys.DTOs.Review_Defects
{
    public class EditReview_DefectsDTO
    {
        public int ID { get; set; }
        public string Review { get; set; }
        public string Possible_Defects { get; set; }
        public int History { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
