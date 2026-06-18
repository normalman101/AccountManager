namespace AccountManager.Application.DTOs;

public record AuthenticateRequest
{
    public required string Email { init; get; }
    public required string Password { init; get; }
}