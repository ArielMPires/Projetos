namespace NexuSys.DTOs.ServiceItems
{
    public class NewServiceItemsDTO
    {
        public int ID { get; set; }
        public int OS { get; set; }
        public int Product { get; set; }
        public int History { get; set; }
        public decimal Value { get; set; }
        public decimal Amount { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }

    }
}
