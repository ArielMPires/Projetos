using NexuSys.Entities;

namespace NexuSys.DTOs.Possible_Defects
{
    public class Possible_DefectsDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int History { get; set; }
        public History historyFK { get; set; }

    }
}
