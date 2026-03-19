namespace NexuSys.Entities
{
    public class Return
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string? Error { get; set; }
        public object? Data { get; set; }
    }
}
