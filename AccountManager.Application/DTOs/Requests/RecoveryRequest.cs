namespace AccountManager.Application.DTOs.Requests;

public sealed record RecoveryRequest
{
    public required string Email { init; get; }
    public required string Password { init; get; }
}