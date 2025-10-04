namespace CST8002.Application.Abstractions
{
    public interface ICurrentUser
    {
        int UserId { get; }
        string Role { get; }
    }
}
