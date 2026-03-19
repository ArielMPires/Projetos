namespace NexuSys.DTOs.Seller
{
    public class NewSellerDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Customers { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
