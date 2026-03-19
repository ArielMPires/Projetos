namespace NexuSys.DTOs.ServiceOrder
{
    public class ServiceOrderDTO
    {
        public int ID { get; set; }
        public string Customer { get; set; }
        public DateTime Date_Receipt { get; set; }
        public string Situation { get; set; }
        public string Department { get; set; }
        public string Problem { get; set; }
        public DateTime Departure_date { get; set; }
        public int Service_Note { get; set; }
        public int Delivery_Note { get; set; }
        public int Departure_Note { get; set; }
        public string? Note { get; set; }
        public int FileFolder { get; set; }
        public int History { get; set; }
        public string Unit { get; set; }
        public string Technical { get; set; }
        public string Type_Service { get; set; }
        public DateTime Estimated_Date { get; set; }
        public int Priority { get; set; }
    }
}
