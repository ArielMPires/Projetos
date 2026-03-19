namespace Domus.DTO.Purchase_Order
{
    public class Purchase_OrderDTO
    {
        public int ID { get; set; }
        public int Request { get; set; }
        public string Requester { get; set; }
        public string Supplier { get; set; }
        public string Situation { get; set; }
        public DateTime Delivery_Time { get; set; }
        public decimal Value { get; set; }
        public string Note { get; set; }
    }
}


