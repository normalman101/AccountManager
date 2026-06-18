namespace AccountManager.Application.DTOs;

public record RegisterRequest
{
    public required string Email { init; get; }
    public required string Password { init; get; }
};