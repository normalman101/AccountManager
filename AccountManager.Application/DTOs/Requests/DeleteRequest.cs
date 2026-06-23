namespace AccountManager.Application.DTOs.Requests;

public record DeleteRequest
{
    public required string Email { init; get; }
}