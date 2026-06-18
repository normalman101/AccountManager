namespace AccountManager.Application.DTOs;

public record RecoveryRequest
{
    public required string Email { init; get; }
    public required string Password { init; get; }
};