namespace Domus.DTO.Service_Order
{
    public class EndOrderDTO
    {
        public bool Status { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
        public string Reason { get; set; }
    }
}
