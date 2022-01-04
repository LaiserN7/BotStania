using Domain.Enums;

namespace Domain.Exceptions;

public class BaseException : Exception
{
    public BaseException(ErrorCode errorCode, string message) : base($"{errorCode}: {message}")
    {
    }
}