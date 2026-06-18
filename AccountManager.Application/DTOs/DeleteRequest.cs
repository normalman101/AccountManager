namespace AccountManager.Application.DTOs;

public record DeleteRequest
{
    public required string Email { init; get; }
}