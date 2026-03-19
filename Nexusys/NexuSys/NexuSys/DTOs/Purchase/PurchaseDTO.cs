namespace NexuSys.DTOs.Purchase
{
    public class PurchaseDTO
    {
        public int ID { get; set; }
        public int NF   { get; set; }
        public int Supplier { get; set; }
        public DateTime Date { get; set; }
        public Decimal Value_total { get; set; }
    }
}
