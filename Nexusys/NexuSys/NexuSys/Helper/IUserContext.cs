namespace NexuSys.Helper
{
    public interface IUserContext
    {
        int UserId { get; }
        string UserName { get; }
        bool IsAuthenticated { get; }
    }
}
