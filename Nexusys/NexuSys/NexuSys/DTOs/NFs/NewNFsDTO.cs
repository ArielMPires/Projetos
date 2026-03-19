namespace NexuSys.DTOs.Nfs
{
    public class NewNFsDTO
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public int Customers { get; set; }
        public int Type { get; set; }
        public DateTime DateIn { get; set; }
        public decimal? Total_Value { get; set; }
        public decimal Tax { get; set; }
        public int? Folder { get; set; }
    }
}
