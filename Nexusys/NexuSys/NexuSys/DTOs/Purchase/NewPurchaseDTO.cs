using NexuSys.DTOs.Purchase_Items;

namespace NexuSys.DTOs.Purchase
{
    public class NewPurchaseDTO
    {
        public int ID { get; set; }
        public int  NF { get; set; }
        public int Supplier { get; set; }
        public List<NewPurchase_ItemsDTO> Items { get; set; }

        public DateTime Date { get; set; }
        public Decimal Value_total { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
