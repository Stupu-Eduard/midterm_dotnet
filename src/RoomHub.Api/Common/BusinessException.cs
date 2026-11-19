namespace RoomHub.Api.Common;

// Level A: business errors are still exceptions. Replaced by Result<T> in level B.
public sealed class BusinessException(string code, string message, int statusCode)
    : Exception(message)
{
    public string Code { get; } = code;
    public int StatusCode { get; } = statusCode;
}
