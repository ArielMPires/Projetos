namespace NexuSys.DTOs.Equipment
{
    public class NewEquipmentDTO
    {
        public int ID { get; set; }
        public int? Serial { get; set; }
        public DateTime Manufacturing_Date { get; set; }
        public int History { get; set; }
        public string? Optional { get; set; }
        public int Customer { get; set; }
        public int Product { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
