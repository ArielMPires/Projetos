namespace NexuSys.DTOs.Review_Defects
{
    public class NewReview_DefectsDTO
    {
        public int ID { get; set; }
        public int Review { get; set; }
        public int Defects { get; set; }
        public int History { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
