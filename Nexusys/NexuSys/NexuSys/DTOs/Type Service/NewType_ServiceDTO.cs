
namespace NexuSys.DTOs.Type_Service
{
    public class NewType_ServiceDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int History { get; set; }
        public int CreateBy { get; set; }
        public DateTime DateCreate { get; internal set; }
    }
}
