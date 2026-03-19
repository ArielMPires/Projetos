using NexuSys.Entities;

namespace NexuSys.DTOs.Equipment
{
    public class EquipmentDTO
    {
        public int ID { get; set; }
        public int Serial { get; set; }
        public DateTime Manufacturing_Date { get; set; }
        public int History { get; set; }
        public string Optional { get; set; }
        public string Customer { get; set; }
        public string Product { get; set; }
    }
}
