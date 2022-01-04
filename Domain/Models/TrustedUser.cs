namespace Domain.Models;

public class TrustedUser
{
    public TrustedUser(string name)
    {
        Name = name;
    }

    public TrustedUser(long userId)
    {
        UserId = userId;
    }
    
    public TrustedUser(string name, long userId)
    {
        Name = name;
        UserId = userId;
    }

    public DateTime AddedAt { get; init; } = DateTime.UtcNow;
    public string Name { get; init; } = "unknown";
    public long UserId { get; init; }
}