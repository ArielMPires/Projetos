namespace NexuSys.DTOs.ServiceEquipment
{
    public class ByServiceEquipmentDTO
    {
        public int ID { get; set; }
        public int OS { get; set; }
        public string EquipmentCompleto { get; set; }
        public int Serial { get; set; }
        public string Equipment { get; set; }
        public int Product { get; set; }
        public string? Optional { get; set; }
        public DateTime Manufacturing_Date { get; set; }
    }
}
