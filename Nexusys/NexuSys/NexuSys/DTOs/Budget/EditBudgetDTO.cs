namespace NexuSys.DTOs.Budget
{
    public class EditBudgetDTO
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public bool Situation { get; set; }
        public decimal Value { get; set; }
        public int FileFolder { get; set; }
        public int History { get; set; }
        public string Service_Order { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
