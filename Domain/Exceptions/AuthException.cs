using Domain.Enums;

namespace Domain.Exceptions;

public class AuthException : BaseException
{
    public AuthException(string message) : base(ErrorCode.Auth, message)
    {
    }
}