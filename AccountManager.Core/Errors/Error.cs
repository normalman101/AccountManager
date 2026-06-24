namespace AccountManager.Core.Errors;

public sealed class Error(ErrorCode code, string message)
{
    public ErrorCode Code { get; } = code;
    public string Message { get; } = message;
}