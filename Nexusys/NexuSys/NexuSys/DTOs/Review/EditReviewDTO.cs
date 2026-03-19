namespace NexuSys.DTOs.Review
{
    public class EditReviewDTO
    {
        public int ID { get; set; }
        public int Budget { get; set; }
        public int Budget_number { get; set; }
        public int Type_Defect { get; set; }
        public string Logged_Faults { get; set; }
        public string Defect_Reported { get; set; }
        public int History { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
