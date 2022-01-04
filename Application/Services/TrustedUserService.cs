using System.Collections.Concurrent;
using Application.Abstractions;
using Domain.Models;

namespace Application.Services;

public class TrustedUserService : ITrustedUsersService
{
    private ConcurrentDictionary<long, TrustedUser> TrustedUsers = new();

    public bool Add(TrustedUser user) => TrustedUsers.TryAdd(user.UserId, user);

    public bool Remove(long userId) => TrustedUsers.TryRemove(userId, out _);

    public bool Remove(string name)
    {
        var element = TrustedUsers.Values.FirstOrDefault(x => x.Name == name);
        return element is not null && TrustedUsers.TryRemove(element.UserId, out _);
    }

    public bool IsExist(long userId) => TrustedUsers.ContainsKey(userId);

    public bool IsExist(string name) => TrustedUsers.Values.Any(x => x.Name == name);
    public bool IsEmpty() => TrustedUsers.IsEmpty;
}