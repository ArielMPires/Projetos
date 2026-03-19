namespace NexuSys.DTOs.Repair_Activities
{
    public class EditRepair_ActivitiesDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int History { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
