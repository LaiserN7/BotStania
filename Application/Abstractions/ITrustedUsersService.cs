using Domain.Models;

namespace Application.Abstractions;

public interface ITrustedUsersService
{
    bool Add(TrustedUser user);
    bool Remove(long userId);
    bool Remove(string name);
    bool IsExist(long userId);
    bool IsExist(string name);
    bool IsEmpty();
}