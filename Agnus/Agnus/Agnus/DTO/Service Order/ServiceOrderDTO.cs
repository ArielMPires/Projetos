namespace Domus.DTO.Service_Order
{
    public class ServiceOrderDTO
    {
        public int ID { get; set; }
        public string Problem { get; set; }
        public string Request {  get; set; }
        public int Computer {  get; set; }
        public DateTime Requested_Date { get; set; }
        public string? Technical {  get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
        public int Priority { get; set; }
        public string Category { get; set; }
        public DateTime? Contact_Date { get; set; }
        public string? Note { get; set; }
        public string? Service { get; set; }
        public DateTime? Conclude_Date { get; set; }
    }
}
