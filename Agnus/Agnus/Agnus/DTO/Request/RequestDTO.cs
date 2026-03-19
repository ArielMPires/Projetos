namespace Agnus.DTO.Request
{
    public class RequestDTO
    {
        public int ID { get; set; }
        public string Requester { get; set; }
        public string Department { get; set; }
        public string Use { get; set; }
        public decimal Value { get; set; }
        public string Note { get; set; }
        public bool? isAuthorization { get; set; }
    }
}
