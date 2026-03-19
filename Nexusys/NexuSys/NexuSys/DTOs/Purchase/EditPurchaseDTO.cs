namespace NexuSys.DTOs.Purchase
{
    public class EditPurchaseDTO
    {
        public int ID { get; set; }
        public int NF { get; set; }
        public int Supplier { get; set; }
        public DateTime Date { get; set; }
        public Decimal Value_total { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int ChangedBy { get; set; }
        public DateTime? DateChanged { get; set; }
    }
}
