namespace NexuSys.DTOs.ServiceOrder
{
    public class EditServiceOrderDTO
    {
        public int ID { get; set; }
        public int Customer { get; set; }
        public DateTime Date_Receipt { get; set; }
        public int Situation { get; set; }
        public int Department { get; set; }
        public string Problem { get; set; }
        public DateTime? Departure_date { get; set; }
        public int? Service_Note { get; set; }
        public int? Delivery_Note { get; set; }
        public int? Departure_Note { get; set; }
        public string Note { get; set; }
        public int FileFolder { get; set; }
        public int History { get; set; }
        public int Unit { get; set; }
        public int? Technical { get; set; }
        public int? Type_Service { get; set; }
        public DateTime? Estimated_Date { get; set; }
        public int Priority { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
