namespace NexuSys.DTOs.Budget
{
    public class NewBudgetDTO
    {
        public int ID { get; set; }
        public int OS { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public bool? Situation { get; set; }
        public decimal Value { get; set; }
        public string Note { get; set; }
        public int FileFolder { get; set; }
        public int History { get; set; }
        public string Service_Order { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
