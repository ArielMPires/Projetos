namespace NexuSys.DTOs.Purchase_Items
{
    public class ByPurchase_ItemsDTO
    {
        public int ID { get; set; }
        public int Purchase { get; set; }
        public int Products { get; set; }
        public decimal Value { get; set; }
        public decimal Amount { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
        public int ChangedBy { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
