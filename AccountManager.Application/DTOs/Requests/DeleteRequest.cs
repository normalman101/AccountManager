namespace AccountManager.Application.DTOs.Requests;

public sealed record DeleteRequest
{
    public required string Email { init; get; }
}