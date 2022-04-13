using System.Collections.Concurrent;
using Application.Abstractions;
using Domain.Models;

namespace Application.Services;

public class TrustedUserService : ITrustedUsersService
{
    private readonly ConcurrentDictionary<long, TrustedUser> _trustedUsers = new();

    public bool Add(TrustedUser user) => _trustedUsers.TryAdd(user.UserId, user);

    public bool Remove(long userId) => _trustedUsers.TryRemove(userId, out _);

    public bool Remove(string name)
    {
        var element = _trustedUsers.Values.FirstOrDefault(x => x.Name == name);
        return element is not null && _trustedUsers.TryRemove(element.UserId, out _);
    }

    public bool IsExist(long userId) => _trustedUsers.ContainsKey(userId);

    public bool IsExist(string name) => _trustedUsers.Values.Any(x => x.Name == name);
    public bool IsEmpty() => _trustedUsers.IsEmpty;
}